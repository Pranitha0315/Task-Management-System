using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nest;
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
            var result = _taskService.GetAllTasks();
            return Ok(result);
        }
        [HttpGet("{TaskId}")]
        public IActionResult GetTaskById(int TaskId)
        {
            var result = _taskService.GetTaskById(TaskId);
            return Ok(result);
        }
        [HttpGet("Title")]
        public IActionResult SearchTasks(string Title)
        {
            var result = _taskService.SearchTasks(Title);
            return Ok(result);
        }
        [HttpPost]
        public IActionResult AddTask([FromBody] CreateTaskItemDto dto)
        {
            var result = _taskService.AddTask(dto);
            return Ok(result);
        }
        [HttpPut("{TaskId}")]
        public IActionResult UpdateTask(int TaskId, [FromBody] UpdateTaskItemDto dto)
        {
            var result = _taskService.UpdateTask(TaskId, dto);
            return Ok(result);

        }
        [HttpPut("{TaskId}")]
        public IActionResult ChangeStatus(int TaskId, string Status)
        {
            var result = _taskService.ChangeStatus(TaskId, Status);
            return Ok(result);
        }

        //[HttpDelete("{TaskId}")]
        //public IActionResult DeleteTask(int TaskId)
        //{
        //    var result = _taskService.DeleteTask(TaskId);
        //    return Ok(result);
        //}


    }
}
