using Microsoft.AspNetCore.Mvc;
using Task_Management_System.DTOs;
using Task_Management_System.Services;

namespace Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(_userService.GetAllUsers());
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            UserResponseDto? user = _userService.GetUserById(userId);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpGet("{userId}/tasks")]
        public IActionResult GetUserWithTasks(int userId)
        {
            ApiResponse<UserWithTasksDto> result = _userService.GetUserWithTasks(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "User not found." ? NotFound(result) : BadRequest(result);
        }

        [HttpPost]
        public IActionResult AddUser([FromBody] CreateUserDto dto)
        {
            ApiResponse<UserResponseDto> result = _userService.AddUser(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UpdateUserDto dto)
        {
            ApiResponse<UserResponseDto> result = _userService.UpdateUser(userId, dto);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "User not found." ? NotFound(result) : BadRequest(result);
        }

        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            ApiResponse<UserResponseDto> result = _userService.DeleteUser(userId);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "User not found." ? NotFound(result) : BadRequest(result);
        }
    }
}
