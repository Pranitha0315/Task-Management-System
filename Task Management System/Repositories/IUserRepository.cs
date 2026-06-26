using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        User GetUserById(int UserId);
        List<User> AddTask(int UserId, string UserName, string Email);
        User GetUserWithTasks(int UserId);

        User AddUser(string userName, ApiResponse userlist);
        User AddUser(string userName, CreateUserDto userlist);
        bool EmailExit(string email);
        int AddUser(string userName, string email);
    }
}
