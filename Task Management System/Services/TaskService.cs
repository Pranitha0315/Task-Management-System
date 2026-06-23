using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository TaskRepository)
        {
            _taskRepository = TaskRepository;
        }

        List<TaskItemResponseDto> GetAllTasks()
        {
            List<TaskItem> TaskItemlist = _taskRepository.GetAllTasks();
            List<TaskItemResponseDto> TaskItemlistDtos = new List<TaskItemResponseDto>();
            foreach (var TaskItemlists in TaskItemlist)
            {
                TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
                {
                    TaskId = TaskItemlists.TaskId,
                    Title = TaskItemlists.Title,
                    Description = TaskItemlists.Description,
                    Status = TaskItemlists.Status
                };
                TaskItemlistDtos.Add(TaskItemlistDto);



            }
            return TaskItemlistDtos;
        }


        TaskItemResponseDto GetTaskById(int TaskId)
        {


            TaskItem TaskItemlists = _taskRepository.GetTaskById(TaskId);
            if (TaskItemlists == null)
            {
                return null;
            }

            TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
            {
                TaskId = TaskItemlists.TaskId,
                Title = TaskItemlists.Title,
                Description = TaskItemlists.Description,
                Status = TaskItemlists.Status
            };


            return TaskItemlistDto;



        }

        TaskItemResponseDto SearchTasks(string Title)
        {


            TaskItem TaskItemlists = _taskRepository.SearchTasks(Title);
            if (TaskItemlists == null)
            {
                return null;
            }

            TaskItemResponseDto TaskItemlistDto = new TaskItemResponseDto
            {
                
                Status = TaskItemlists.Status
            };


            return TaskItemlistDto;

        }
        List<TaskItemResponseDto> AddTask(int TaskId, string Title, string Description, string Status, int UserId)


    }
}



