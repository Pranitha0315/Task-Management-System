using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface ITaskRepository
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(int TaskID);
        TaskItem SearchTasks(string Title);
      
        bool ChangeStatus(string Status, int TaskID);
        void DeleteTask(int TaskID);
        TaskItem AddTask(int taskId, TaskItemResponseDto newTask);
    
        TaskItem ChangeStatus(TaskItem changeStatus);
   
        TaskItem UpdateTask(UpdateTaskItemDto updatedTask);
        User GetTaskById(List<TaskItem> taskId);
        bool TaskExits(int taskId);
        void UpdateTask(int taskId, string title, string status, string description, int userID);
        int AddTask(int taskId, string? title, string? status, string description, int userID);
    }
}
