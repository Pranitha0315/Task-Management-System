using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface ITaskRepository
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(int TaskID);
        TaskItem SearchTasks(string Title);
        List<TaskItem> AddTask(int TaskID, string Title, string Description, string Status, int UserId);
        List<TaskItem> UpdateTask(string Title, string Description, string Status, int UserId);
        bool ChangeStatus(string Status, int TaskID);
        void DeleteTask(int TaskID);
        TaskItem AddTask(TaskItemResponseDto newTask);
        TaskItem UpdateTask(TaskItemResponseDto updatedTask);
        TaskItem ChangeStatus(TaskItem changeStatus);
        TaskItem DeleteTask(object taskId);
        TaskItem UpdateTask(UpdateTaskItemDto updatedTask);
    }
}
