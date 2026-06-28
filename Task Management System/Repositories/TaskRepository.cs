using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Task_Management")
                ?? throw new InvalidOperationException("Connection string 'Task_Management' not found.");
        }

        public List<TaskItem> GetAllTasks()
        {
            const string query = "SELECT TaskId, Title, Description, Status, CreatedDate, UserId FROM Tasks";
            return ExecuteTaskQuery(query);
        }

        public TaskItem? GetTaskById(int taskId)
        {
            const string query = "SELECT TaskId, Title, Description, Status, CreatedDate, UserId FROM Tasks WHERE TaskId = @TaskId";
            return ExecuteTaskQuery(query, new SqlParameter("@TaskId", taskId)).FirstOrDefault();
        }

        public List<TaskItem> SearchByTitle(string title)
        {
            const string query = @"SELECT TaskId, Title, Description, Status, CreatedDate, UserId
                                   FROM Tasks
                                   WHERE Title LIKE '%' + @Title + '%'";
            return ExecuteTaskQuery(query, new SqlParameter("@Title", title));
        }

        public int AddTask(CreateTaskItemDto dto)
        {
            const string query = @"
                INSERT INTO Tasks (Title, Description, Status, UserId)
                VALUES (@Title, @Description, @Status, @UserId);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Title", dto.Title);
            command.Parameters.AddWithValue("@Description", dto.Description);
            command.Parameters.AddWithValue("@Status", dto.Status);
            command.Parameters.AddWithValue("@UserId", dto.UserId);

            connection.Open();
            return (int)command.ExecuteScalar()!;
        }

        public void UpdateTask(int taskId, UpdateTaskItemDto dto)
        {
            const string query = @"
                UPDATE Tasks
                SET Title = @Title, Description = @Description, Status = @Status, UserId = @UserId
                WHERE TaskId = @TaskId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@Title", dto.Title);
            command.Parameters.AddWithValue("@Description", dto.Description);
            command.Parameters.AddWithValue("@Status", dto.Status);
            command.Parameters.AddWithValue("@UserId", dto.UserId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void UpdateStatus(int taskId, string status)
        {
            const string query = "UPDATE Tasks SET Status = @Status WHERE TaskId = @TaskId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);
            command.Parameters.AddWithValue("@Status", status);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public bool TaskExists(int taskId)
        {
            const string query = "SELECT COUNT(1) FROM Tasks WHERE TaskId = @TaskId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);

            connection.Open();
            return (int)command.ExecuteScalar()! > 0;
        }

        public void DeleteTask(int taskId)
        {
            const string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private List<TaskItem> ExecuteTaskQuery(string query, params SqlParameter[] parameters)
        {
            List<TaskItem> tasks = new();

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                tasks.Add(MapTask(reader));
            }

            return tasks;
        }

        private static TaskItem MapTask(SqlDataReader reader) => new()
        {
            TaskId = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.GetString(2),
            Status = reader.GetString(3),
            CreatedDate = reader.GetDateTime(4),
            UserId = reader.GetInt32(5)
        };
    }
}
