using Task_Management_System.DTOs;

namespace Task_Management_System.Services
{
    public interface ITaskService
    {
        List<TaskItemResponseDto> GetAllTasks();
        TaskItemResponseDto? GetTaskById(int taskId);
        List<TaskItemResponseDto> SearchTasks(string title);
        ApiResponse<TaskItemResponseDto> AddTask(CreateTaskItemDto dto);
        ApiResponse<TaskItemResponseDto> UpdateTask(int taskId, UpdateTaskItemDto dto);
        ApiResponse<TaskItemResponseDto> ChangeStatus(int taskId, string status);
        ApiResponse<TaskItemResponseDto> DeleteTask(int taskId);
    }
}
