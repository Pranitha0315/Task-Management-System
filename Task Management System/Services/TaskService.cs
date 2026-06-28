using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public List<TaskItemResponseDto> GetAllTasks()
        {
            return _taskRepository.GetAllTasks().Select(MapToDto).ToList();
        }

        public TaskItemResponseDto? GetTaskById(int taskId)
        {
            TaskItem? task = _taskRepository.GetTaskById(taskId);
            return task == null ? null : MapToDto(task);
        }

        public List<TaskItemResponseDto> SearchTasks(string title)
        {
            return _taskRepository.SearchByTitle(title).Select(MapToDto).ToList();
        }

        public ApiResponse<TaskItemResponseDto> AddTask(CreateTaskItemDto dto)
        {
            List<string> errors = ValidateTask(dto.Title, dto.Description, dto.Status, dto.UserId);
            if (errors.Count > 0)
            {
                return Fail<TaskItemResponseDto>("Validation error.", errors);
            }

            if (!_userRepository.UserExists(dto.UserId))
            {
                return Fail<TaskItemResponseDto>("Validation error.", new List<string> { "User not found." });
            }

            try
            {
                int taskId = _taskRepository.AddTask(dto);
                TaskItem? task = _taskRepository.GetTaskById(taskId);
                return Success(MapToDto(task!), "Task created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<TaskItemResponseDto>(ex.Message);
            }
        }

        public ApiResponse<TaskItemResponseDto> UpdateTask(int taskId, UpdateTaskItemDto dto)
        {
            if (taskId <= 0)
            {
                return Fail<TaskItemResponseDto>("Invalid task id.");
            }

            List<string> errors = ValidateTask(dto.Title, dto.Description, dto.Status, dto.UserId);
            if (errors.Count > 0)
            {
                return Fail<TaskItemResponseDto>("Validation error.", errors);
            }

            if (!_taskRepository.TaskExists(taskId))
            {
                return Fail<TaskItemResponseDto>("Task not found.");
            }

            if (!_userRepository.UserExists(dto.UserId))
            {
                return Fail<TaskItemResponseDto>("Validation error.", new List<string> { "User not found." });
            }

            try
            {
                _taskRepository.UpdateTask(taskId, dto);
                TaskItem? task = _taskRepository.GetTaskById(taskId);
                return Success(MapToDto(task!), "Task updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<TaskItemResponseDto>(ex.Message);
            }
        }

        public ApiResponse<TaskItemResponseDto> ChangeStatus(int taskId, string status)
        {
            if (taskId <= 0)
            {
                return Fail<TaskItemResponseDto>("Invalid task id.");
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                return Fail<TaskItemResponseDto>("Status is required.");
            }

            if (!_taskRepository.TaskExists(taskId))
            {
                return Fail<TaskItemResponseDto>("Task not found.");
            }

            try
            {
                _taskRepository.UpdateStatus(taskId, status);
                TaskItem? task = _taskRepository.GetTaskById(taskId);
                return Success(MapToDto(task!), "Task status updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<TaskItemResponseDto>(ex.Message);
            }
        }

        public ApiResponse<TaskItemResponseDto> DeleteTask(int taskId)
        {
            if (taskId <= 0)
            {
                return Fail<TaskItemResponseDto>("Invalid task id.");
            }

            if (!_taskRepository.TaskExists(taskId))
            {
                return Fail<TaskItemResponseDto>("Task not found.");
            }

            try
            {
                _taskRepository.DeleteTask(taskId);
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = true,
                    Message = "Task deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return Fail<TaskItemResponseDto>(ex.Message);
            }
        }

        private static List<string> ValidateTask(string title, string description, string status, int userId)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(title))
                errors.Add("Title is required.");
            if (string.IsNullOrWhiteSpace(description))
                errors.Add("Description is required.");
            if (string.IsNullOrWhiteSpace(status))
                errors.Add("Status is required.");
            if (userId <= 0)
                errors.Add("Valid user id is required.");

            return errors;
        }

        private static TaskItemResponseDto MapToDto(TaskItem task) => new()
        {
            TaskId = task.TaskId,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            CreatedDate = task.CreatedDate,
            UserId = task.UserId
        };

        private static ApiResponse<T> Success<T>(T data, string message) => new()
        {
            Success = true,
            Message = message,
            Data = data
        };

        private static ApiResponse<T> Fail<T>(string message, List<string>? errors = null) => new()
        {
            Success = false,
            Message = message,
            Error = errors ?? new List<string> { message }
        };
    }
}
