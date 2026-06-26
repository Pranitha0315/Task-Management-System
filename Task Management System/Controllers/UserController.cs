using Microsoft.AspNetCore.Http;
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
            var result = _userService.GetAllUsers();
            return Ok(result);
        }
        [HttpGet("{UserId}")]
        public IActionResult GetUserById(int UserId)
        {
            var result = _userService.GetUserById(UserId);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddUser([FromBody] CreateUserDto dto)
        {
            var result = _userService.AddUser(dto);
            return Ok(result);
        }
        [HttpGet]
        public IActionResult GetUserWithTasks(int UserId)
        {
            var result = _userService.GetUserWithTasks(UserId);
            return Ok(result);
        }
    }
}
