using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserResponseDto> GetAllUsers()
        {
            return _userRepository.GetAllUsers().Select(MapToDto).ToList();
        }

        public UserResponseDto? GetUserById(int userId)
        {
            User? user = _userRepository.GetUserById(userId);
            return user == null ? null : MapToDto(user);
        }

        public ApiResponse<UserResponseDto> AddUser(CreateUserDto dto)
        {
            List<string> errors = ValidateUser(dto.UserName, dto.Email);
            if (errors.Count > 0)
            {
                return Fail<UserResponseDto>("Validation error.", errors);
            }

            if (_userRepository.EmailExists(dto.Email))
            {
                return Fail<UserResponseDto>("Validation error.", new List<string> { "Email already exists." });
            }

            try
            {
                int userId = _userRepository.AddUser(dto.UserName, dto.Email);
                User? user = _userRepository.GetUserById(userId);
                return Success(MapToDto(user!), "User created successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.Message);
            }
        }

        public ApiResponse<UserResponseDto> UpdateUser(int userId, UpdateUserDto dto)
        {
            if (userId <= 0)
            {
                return Fail<UserResponseDto>("Invalid user id.");
            }

            List<string> errors = ValidateUser(dto.UserName, dto.Email);
            if (errors.Count > 0)
            {
                return Fail<UserResponseDto>("Validation error.", errors);
            }

            if (!_userRepository.UserExists(userId))
            {
                return Fail<UserResponseDto>("User not found.");
            }

            try
            {
                _userRepository.UpdateUser(userId, dto.UserName, dto.Email);
                User? user = _userRepository.GetUserById(userId);
                return Success(MapToDto(user!), "User updated successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.Message);
            }
        }

        public ApiResponse<UserWithTasksDto> GetUserWithTasks(int userId)
        {
            if (userId <= 0)
            {
                return Fail<UserWithTasksDto>("Invalid user id.");
            }

            try
            {
                User? user = _userRepository.GetUserWithTasks(userId);
                if (user == null)
                {
                    return Fail<UserWithTasksDto>("User not found.");
                }

                return Success(new UserWithTasksDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    Email = user.Email,
                    Tasks = user.Tasks.Select(t => new TaskItemResponseDto
                    {
                        TaskId = t.TaskId,
                        Title = t.Title,
                        Description = t.Description,
                        Status = t.Status,
                        CreatedDate = t.CreatedDate,
                        UserId = t.UserId
                    }).ToList()
                }, "User with tasks retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserWithTasksDto>(ex.Message);
            }
        }

        public ApiResponse<UserResponseDto> DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                return Fail<UserResponseDto>("Invalid user id.");
            }

            User? user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                return Fail<UserResponseDto>("User not found.");
            }

            try
            {
                _userRepository.DeleteUser(userId);
                return Success(MapToDto(user), "User deleted successfully.");
            }
            catch (Exception ex)
            {
                return Fail<UserResponseDto>(ex.Message);
            }
        }

        private static List<string> ValidateUser(string userName, string email)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(userName))
                errors.Add("UserName is required.");
            if (string.IsNullOrWhiteSpace(email))
                errors.Add("Email is required.");

            return errors;
        }

        private static UserResponseDto MapToDto(User user) => new()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            Email = user.Email
        };

        private static ApiResponse<T> Success<T>(T data, string message) => new()
        {
            Success = true,
            Message = message,
            Data = data
        };

        private static ApiResponse<T> Fail<T>(string message, List<string>? errors = null) => new()
        {
            Success = false,
            Message = message,
            Error = errors ?? new List<string> { message }
        };
    }
}
