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


    }
}
