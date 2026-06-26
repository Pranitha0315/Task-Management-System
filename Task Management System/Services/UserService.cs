using Google.Api.Ads.AdWords.v201809;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.Reflection.PortableExecutable;
using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;


namespace Task_Management_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly object _taskRepository;

        public UserService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }
        public List<ApiResponse> GetAllUsers()
        {
            List<User> Userlist = _UserRepository.GetAllUsers();
            List<ApiResponse> UserlistSDtos = new List<ApiResponse>();
            foreach (var Details in Userlist)
            {
                ApiResponse UserlistSDto = new ApiResponse
                {
                    UserId = Details.UserId,
                    UserName = Details.UserName,
                    Email = Details.Email
                };

                UserlistSDtos.Add(UserlistSDto);


            }
            return UserlistSDtos;
        }



        public ApiResponse GetUserById(int UserId)
        {
            User Details = _UserRepository.GetUserById(UserId);
            if (Details == null)
            {
                return null;
            }
            ApiResponse UserlistSDto = new ApiResponse
            {
                UserId = Details.UserId,
                UserName = Details.UserName,
                Email = Details.Email
            };
            return UserlistSDto;
        }

        ApiResponse<User> IUserService.AddUser(CreateUserDto dto)
        {
                var errors = ValidateUser(dto);
            if (errors != null && errors.Count > 0)
            {
                return new ApiResponse<User>
                {
                    Success = false,
                    Message = "Validation Error",
                    Error = errors
                };
            }
            try
            {
                if (_UserRepository.EmailExit(dto.Email))
                {
                    return new ApiResponse<User>
                    {
                        Success = false,
                        Message = "Validation Error",
                        Error = new List<string> { "Valid unique email is required" }
                    };
                }
                var userid = _UserRepository.AddUser(dto.UserName, dto.Email);
                var user = _UserRepository.GetUserById(userid);
                return new ApiResponse<User>
                {
                    Success = true,
                    Message = "User created successfully.",
                    Data = user
                };
            }
            catch (Exception ex)
            {
                return ErrorResponse<User>(ex.Message);
            }
        }

        private List<string> ValidateUser(CreateUserDto dto)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(dto.UserName))
                errors.Add("UserName is required.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                errors.Add("Email is required.");
            // Add more validation as needed
            return errors;
        }

        ApiResponse<UserWithTasksDto> IUserService.GetUserWithTasks(int UserId)
        {
            if (UserId <= 0)
            {
                return ValidationError<UserWithTasksDto>("Invaild User Id");
            }
            try
            {
                User Details = _UserRepository.GetUserWithTasks(UserId);

                if (Details == null)
                {
                    return NotFound<UserWithTasksDto>("User not found");
                }
                return new ApiResponse<UserWithTasksDto>
                {
                    Success = true,
                    Message = "User with task recived successfully",
                    Data = Details
                };
            }
            catch
            {
                return ErrorResponse<UserWithTasksDto>("found error");
            }

            

        }

        private ApiResponse<T> ErrorResponse<T>(string error)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "An error occurred.",
                Error = new List<string> { error }
            };
        }

        private ApiResponse<T> NotFound<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message,
                Error = new List<string> { message }
            };
        }

        private ApiResponse<T> ValidationError<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Validation Error",
                Error = new List<string> { message }
            };
        }

        public List<CreateUserDto> AddUser(int UserId, string UserName, string Email, List<CreateUserDto> userlist)
        {
            throw new NotImplementedException();
        }

    }
}

//List<TaskItemResponseDto> tasks = _taskRepository.GetUserWithTasks(UserId);


//UserWithTasksDto userWithTasksDto = new UserWithTasksDto
//{
//    UserId = Details.UserId,
//    UserName = Details.UserName,
//    Email = Details.Email,
//    Tasks = tasks.Select(t => new TaskItemResponseDto
//    {
//        TaskId = t.TaskId,
//        Title = t.Title,
//        Description = t.Description,
//        Status = t.Status
//    }).ToList()
//};
