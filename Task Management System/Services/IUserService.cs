using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Services;

namespace Task_Management_System.Services
{
    public interface IUserService
    {
        public List<ApiResponse> GetAllUsers();

        public ApiResponse GetUserById(int UserId);
        public List<CreateUserDto> AddUser(int UserId, string UserName, string Email, List<CreateUserDto> userlist);
        ApiResponse<UserWithTasksDto> GetUserWithTasks(int UserId);
        ApiResponse<User> AddUser(CreateUserDto dto);
    }
}

