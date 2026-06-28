using Task_Management_System.DTOs;

namespace Task_Management_System.Services
{
    public interface IUserService
    {
        List<UserResponseDto> GetAllUsers();
        UserResponseDto? GetUserById(int userId);
        ApiResponse<UserResponseDto> AddUser(CreateUserDto dto);
        ApiResponse<UserResponseDto> UpdateUser(int userId, UpdateUserDto dto);
        ApiResponse<UserWithTasksDto> GetUserWithTasks(int userId);
        ApiResponse<UserResponseDto> DeleteUser(int userId);
    }
}
