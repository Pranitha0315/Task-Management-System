using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;
using Task_Management_System.Repositories;

namespace Task_Management_System.Services
{
    public class TaskService : ITaskRepository
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
        List<TaskItemResponseDto> AddTask(int TaskId, string Title, string Description, string Status, int UserId, List<TaskItemResponseDto> taskDto, TaskItemResponseDto TaskItemlist, List<TaskItemResponseDto> taskItemlist)
        {
            TaskItemResponseDto TaskItemlistDtos= new TaskItemResponseDto
            {
                TaskId = TaskId,
                Title = Title,
                Description = Description,
                Status = Status,
                UserId = UserId
            };



            TaskItemResponseDto taskItemResponseDto = new()
            {
                TaskId = createdTask.TaskId,
                Title = createdTask.Title,
                Description = createdTask.Description,
                Status = createdTask.Status
            };
            return taskItemlist;
        }
        public UpdateTaskItemDto UpdateTask(int taskId, string title, string description, string status, int userId)
        {

            UpdateTaskItemDto updatedTask = new UpdateTaskItemDto
            {
                TaskId = taskId,
                Title = title,
                Description = description,
                Status = status,
                UserId = userId
            };


            TaskItem result = _taskRepository.UpdateTask(updatedTask);

            if (result == null)
            {
                return null;
            }

        
            return new UpdateTaskItemDto
            {
                TaskId = result.TaskId,
                Title = result.Title,
                Description = result.Description,
                Status = result.Status
            };
        }


        public ChangeStatusDto ChangeStatus(int taskId, string newStatus)
        {
            TaskItem changeStatus = _taskRepository.GetTaskById(taskId);

            if (changeStatus == null)
            {
                return null; 
            }

            changeStatus.Status = newStatus;

            TaskItem updatedTask = _taskRepository.ChangeStatus(changeStatus);

         
            return new ChangeStatusDto
            {
                TaskId = updatedTask.TaskId,
              
                Status = updatedTask.Status
            };
        }

        public void DeleteTask(int TaskID)
        {

            TaskItem Deleted = _taskRepository.GetTaskById(TaskID);
            if (Deleted != null)
            {
                _taskRepository.DeleteTask(Deleted);
            }
        
        }












        List<TaskItem> ITaskRepository.GetAllTasks()
        {
            return _taskRepository.GetAllTasks();
        }

        TaskItem ITaskRepository.GetTaskById(int TaskID)
        {
            return _taskRepository.GetTaskById(TaskID);
        }

        TaskItem ITaskRepository.SearchTasks(string Title)
        {
            return _taskRepository.SearchTasks(Title);
        }

        public List<TaskItem> AddTask(int TaskID, string Title, string Description, string Status, int UserId)
        {
            return _taskRepository.AddTask(TaskID, Title, Description, Status, UserId);
        }

        public List<TaskItem> UpdateTask(string Title, string Description, string Status, int UserId)
        {
            return _taskRepository.UpdateTask(Title, Description, Status, UserId);
        }

        public bool ChangeStatus(string Status, int TaskID)
        {
            return _taskRepository.ChangeStatus(Status, TaskID);
        }

       
        public TaskItem AddTask(TaskItemResponseDto newTask)
        {
            return _taskRepository.AddTask(newTask);
        }

        public TaskItem UpdateTask(TaskItemResponseDto updatedTask)
        {
            throw new NotImplementedException();
        }

        public TaskItem ChangeStatus(TaskItem changeStatus)
        {
            throw new NotImplementedException();
        }

        public TaskItem DeleteTask(object taskId)
        {
            throw new NotImplementedException();
        }







