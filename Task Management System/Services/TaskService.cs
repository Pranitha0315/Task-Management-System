using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens.Experimental;

using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        private int TaskId;

        public TaskService(ITaskRepository TaskRepository, IUserRepository userRepository)
        {
            _taskRepository = TaskRepository;
            _userRepository = userRepository;
        }

        List<TaskItemResponseDto> GetAllTasks()
        {
            List<TaskItem> TaskItemlist = _taskRepository.GetAllTasks();
            List<TaskItemResponseDto> TaskItemlistDtos = new List<TaskItemResponseDto>();
            foreach (var TaskItemlists in TaskItemlist)
            {
                TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
                {
                    TaskId = TaskItemlists.TaskId,
                    Title = TaskItemlists.Title,
                    Description = TaskItemlists.Description,
                    Status = TaskItemlists.Status
                };
                TaskItemlistDtos.Add(TaskItemlistDto);



            }
            return TaskItemlistDtos;
        }


        TaskItemResponseDto GetTaskById(int TaskId)
        {


            TaskItem TaskItemlists = _taskRepository.GetTaskById(TaskId);
            if (TaskItemlists == null)
            {
                return null;
            }

            TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
            {
                TaskId = TaskItemlists.TaskId,
                Title = TaskItemlists.Title,
                Description = TaskItemlists.Description,
                Status = TaskItemlists.Status
            };


            return TaskItemlistDto;



        }

        TaskItemResponseDto SearchTasks(string Title)
        {


            TaskItem TaskItemlists = _taskRepository.SearchTasks(Title);
            if (TaskItemlists == null)
            {
                return null;
            }

            TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
            {

                Status = TaskItemlists.Status
            };


            return TaskItemlistDto;

        }

     

         ApiResponse<TaskItemResponseDto> AddTask(CreateTaskItemDto dto)
        {
           var error = ValidateTask(dto.TaskId,dto.Title,dto.Status,dto.Description,dto.UserID);
            if (error.Count > 0)
            {
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = false,
                    Message = "validation Error",
                    Data = error
                };
            }
            try
            {
                var TaskId = _taskRepository.AddTask(dto.TaskId, dto.Title, dto.Status, dto.Description, dto.UserID);
                var Task = _taskRepository.GetTaskById(TaskId);
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = true,
                    Message = "Created successfully",
                    Data = Task
                };

            }
            catch
            {
                return ErrorResponse< TaskItemResponseDto>(error);
            }
        }

       

        ApiResponse<TaskItemResponseDto> UpdateTask( int TaskId, UpdateTaskItemDto dto)
        {
            if(TaskId <= 0)
            {
                return ValidationError<TaskItemResponseDto>("Invaild TaskId.");
            }
            var error = ValidateTask(dto.Title, dto.Status, dto.Description, dto.UserID);

            if (error.Count > 0)
            {
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = true,
                    Message = "validation Error",
                    Data = error
                };
            }
            try
            {
                if (!_taskRepository.TaskExits(TaskId))
                {
                    return NotFound<TaskItemResponseDto>("Task Not Found.");
                }
                 _taskRepository.UpdateTask(TaskId, dto.Title, dto.Status, dto.Description, dto.UserID);
                var Task = _taskRepository.GetTaskById(TaskId);
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = true,
                    Message = "Updated successfully",
                    Data = Task
                };

            }
            catch
            {
                return ErrorResponse<TaskItemResponseDto>(error);
            }
        }

        private ApiResponse<T> NotFound<T>(string v)
        {
            throw new NotImplementedException();
        }

        private User ValidateTask(string title, string status, string description, int userID)
        {
            throw new NotImplementedException();
        }

        private ApiResponse<T> ValidationError<T>(string v)
        {
            throw new NotImplementedException();
        }

        public ChangeStatusDto ChangeStatus(int taskId, string newStatus)
        {
            TaskItem changeStatus = _taskRepository.GetTaskById(taskId);

            if (changeStatus == null)
            {
                return null;
            }

            changeStatus.Status = newStatus;

            TaskItem updatedTask = _taskRepository.ChangeStatus(changeStatus);


            return new ChangeStatusDto
            {
                TaskId = updatedTask.TaskId,

                Status = updatedTask.Status
            };
        }

        ApiResponse<TaskItemResponseDto> DeleteTask(int TaskID)
        {
            if (TaskId <= 0)
            {
                return ValidationError<TaskItemResponseDto>("Invaild TaskId.");
            }
            try
            {
                if(!_taskRepository.TaskExits(TaskID))
                {
                    return NotFound<TaskItemResponseDto>("Task Not Found");
                }
                _taskRepository.DeleteTask(TaskID);
                return new ApiResponse<TaskItemResponseDto>
                {
                    Success = true,
                    Message = "Deleted Successfully",

                };

            }
            catch
            {
                return ErrorResponse<TaskItemResponseDto>("Error");
            }

            
        }

        List<TaskItemResponseDto> ITaskService.GetAllTasks()
        {
            return GetAllTasks();
        }

        TaskItemResponseDto ITaskService.GetTaskById(int TaskId)
        {
            return GetTaskById(TaskId);
        }

        TaskItemResponseDto ITaskService.SearchTasks(string Title)
        {
            return SearchTasks(Title);
        }

        public List<TaskItemResponseDto> AddTask(int TaskId, string Title, string Description, string Status, int UserId, List<TaskItemResponseDto> taskDto, TaskItemResponseDto TaskItemlist, List<TaskItemResponseDto> taskItemlist)
        {
            throw new NotImplementedException();
        }

      
        TaskItemResponseDto ITaskService.ChangeStatus(int taskId, string newStatus)
        {
            throw new NotImplementedException();
        }
        private User ValidateTask(int taskId, string? title, string? status, string description, int userID)
        {
            throw new NotImplementedException();
        }

        private ApiResponse<T> ErrorResponse<T>(object error)
        {
            throw new NotImplementedException();
        }

       
        void ITaskService.DeleteTask(int TaskID)
        {
            if (TaskID <= 0)
            {
                // Optionally, throw an exception or handle invalid TaskID as per your application's error handling policy
                throw new ArgumentException("Invalid TaskId.");
            }
            if (!_taskRepository.TaskExits(TaskID))
            {
                // Optionally, throw an exception or handle not found as per your application's error handling policy
                throw new InvalidOperationException("Task Not Found");
            }
            _taskRepository.DeleteTask(TaskID);
        }

        public TaskItemResponseDto UpdateTask(int taskId, string title, string description, string status, int userId)
        {
            throw new NotImplementedException();
        }
    }
}










       


