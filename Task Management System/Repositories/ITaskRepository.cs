using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface ITaskRepository
    {
        List<TaskItem> GetAllTasks();
        TaskItem? GetTaskById(int taskId);
        List<TaskItem> SearchByTitle(string title);
        int AddTask(CreateTaskItemDto dto);
        void UpdateTask(int taskId, UpdateTaskItemDto dto);
        void UpdateStatus(int taskId, string status);
        bool TaskExists(int taskId);
        void DeleteTask(int taskId);
    }
}
