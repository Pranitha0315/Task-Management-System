using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User? GetUserById(int userId);
        User? GetUserWithTasks(int userId);
        int AddUser(string userName, string email);
        void UpdateUser(int userId, string userName, string email);
        bool EmailExists(string email);
        bool UserExists(int userId);
        void DeleteUser(int userId);
    }
}
