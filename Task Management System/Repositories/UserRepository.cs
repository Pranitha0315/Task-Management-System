using Microsoft.Data.SqlClient;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Task_Management")
                ?? throw new InvalidOperationException("Connection string 'Task_Management' not found.");
        }

        public List<User> GetAllUsers()
        {
            const string query = "SELECT UserId, UserName, Email FROM Users";
            return ExecuteUserQuery(query);
        }

        public User? GetUserById(int userId)
        {
            const string query = "SELECT UserId, UserName, Email FROM Users WHERE UserId = @UserId";
            return ExecuteUserQuery(query, new SqlParameter("@UserId", userId)).FirstOrDefault();
        }

        public User? GetUserWithTasks(int userId)
        {
            User? user = null;

            const string query = @"
                SELECT u.UserId, u.UserName, u.Email,
                       t.TaskId, t.Title, t.Description, t.Status, t.CreatedDate, t.UserId
                FROM Users u
                LEFT JOIN Tasks t ON u.UserId = t.UserId
                WHERE u.UserId = @UserId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                user ??= new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2)
                };

                if (!reader.IsDBNull(3))
                {
                    user.Tasks.Add(new TaskItem
                    {
                        TaskId = reader.GetInt32(3),
                        Title = reader.GetString(4),
                        Description = reader.GetString(5),
                        Status = reader.GetString(6),
                        CreatedDate = reader.GetDateTime(7),
                        UserId = reader.GetInt32(8)
                    });
                }
            }

            return user;
        }

        public int AddUser(string userName, string email)
        {
            const string query = @"
                INSERT INTO Users (UserName, Email)
                VALUES (@UserName, @Email);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            return (int)command.ExecuteScalar()!;
        }

        public void UpdateUser(int userId, string userName, string email)
        {
            const string query = @"
                UPDATE Users
                SET UserName = @UserName, Email = @Email
                WHERE UserId = @UserId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public bool EmailExists(string email)
        {
            const string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@Email", email);

            connection.Open();
            return (int)command.ExecuteScalar()! > 0;
        }

        public bool UserExists(int userId)
        {
            const string query = "SELECT COUNT(1) FROM Users WHERE UserId = @UserId";

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            return (int)command.ExecuteScalar()! > 0;
        }

        public void DeleteUser(int userId)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using (SqlCommand deleteTasks = new("DELETE FROM Tasks WHERE UserId = @UserId", connection, transaction))
                {
                    deleteTasks.Parameters.AddWithValue("@UserId", userId);
                    deleteTasks.ExecuteNonQuery();
                }

                using (SqlCommand deleteUser = new("DELETE FROM Users WHERE UserId = @UserId", connection, transaction))
                {
                    deleteUser.Parameters.AddWithValue("@UserId", userId);
                    deleteUser.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private List<User> ExecuteUserQuery(string query, params SqlParameter[] parameters)
        {
            List<User> users = new();

            using SqlConnection connection = new(_connectionString);
            using SqlCommand command = new(query, connection);
            command.Parameters.AddRange(parameters);

            connection.Open();
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return users;
        }
    }
}
