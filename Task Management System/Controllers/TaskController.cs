using Microsoft.AspNetCore.Mvc;
using Task_Management_System.DTOs;
using Task_Management_System.Services;

namespace Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            return Ok(_taskService.GetAllTasks());
        }

        [HttpGet("{taskId}")]
        public IActionResult GetTaskById(int taskId)
        {
            TaskItemResponseDto? task = _taskService.GetTaskById(taskId);
            return task == null ? NotFound() : Ok(task);
        }

        [HttpGet("search")]
        public IActionResult SearchTasks([FromQuery] string title)
        {
            return Ok(_taskService.SearchTasks(title));
        }

        [HttpPost]
        public IActionResult AddTask([FromBody] CreateTaskItemDto dto)
        {
            ApiResponse<TaskItemResponseDto> result = _taskService.AddTask(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{taskId}")]
        public IActionResult UpdateTask(int taskId, [FromBody] UpdateTaskItemDto dto)
        {
            ApiResponse<TaskItemResponseDto> result = _taskService.UpdateTask(taskId, dto);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "Task not found." ? NotFound(result) : BadRequest(result);
        }

        [HttpPut("{taskId}/status")]
        public IActionResult ChangeStatus(int taskId, [FromQuery] string status)
        {
            ApiResponse<TaskItemResponseDto> result = _taskService.ChangeStatus(taskId, status);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "Task not found." ? NotFound(result) : BadRequest(result);
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(int taskId)
        {
            ApiResponse<TaskItemResponseDto> result = _taskService.DeleteTask(taskId);
            if (result.Success)
            {
                return Ok(result);
            }

            return result.Message == "Task not found." ? NotFound(result) : BadRequest(result);
        }
    }
}
