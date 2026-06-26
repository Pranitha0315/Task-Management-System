using Task_Management_System.DTOs;

namespace Task_Management_System.Services
{
    public interface ITaskService
    {
        List<TaskItemResponseDto> GetAllTasks();

        TaskItemResponseDto GetTaskById(int TaskId);

        TaskItemResponseDto SearchTasks(string Title);

        List<TaskItemResponseDto> AddTask(int TaskId, string Title, string Description, string Status, int UserId, List<TaskItemResponseDto> taskDto, TaskItemResponseDto TaskItemlist, List<TaskItemResponseDto> taskItemlist);

        public TaskItemResponseDto UpdateTask(int taskId, string title, string description, string status, int userId);
        ApiResponse<TaskItemResponseDto> UpdateTask(int TaskId, UpdateTaskItemDto dto);

        TaskItemResponseDto ChangeStatus(int taskId, string newStatus);


        void DeleteTask(int TaskID);
        object AddTask(CreateTaskItemDto dto);
    }
}
