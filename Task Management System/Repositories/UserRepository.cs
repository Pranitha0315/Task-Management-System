using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Task_Management_System.DTOs;
using Task_Management_System.Models;

namespace Task_Management_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionStrings;

        public UserRepository(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetConnectionString("Task_Management");
        }
        public List<User> GetAllUsers()
        {
            List<User> Userlist = new List<User>();



            using SqlConnection connection = new SqlConnection(_connectionStrings);

            connection.Open();
            string query = " SELECT  * FROM User ";
            using (SqlCommand command = new SqlCommand(query, connection))

            using (SqlDataReader reader = command.ExecuteReader())

                while (reader.Read())
                {
                    User Detail = new User
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Email = reader.GetString(2)


                    };
                    Userlist.Add(Detail);
                }


            return Userlist;
        }
        public User GetUserById(int UserId)
        {
            User Detail = null;
            using SqlConnection connection = new SqlConnection(_connectionStrings);

            connection.Open();
            string query = "SELECT  * FROM  User where UserId=@UserId";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", UserId);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Detail = new User()
                    {
                        UserId = reader.GetInt32(0),
                        UserName = reader.GetString(1),
                        Email = reader.GetString(2)

                    };

                }

            }

            return Detail;
        }
        public List<User> AddTask( int UserId,string UserName,string Email)
        {

            List<User> Userlist = new List<User>();
            using SqlConnection connection = new SqlConnection(_connectionStrings);

            connection.Open();
            string query = "INSERT INTO User (UserId,UserName,Email) VALUES(@UserId,@UserName,@Email)";
            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@UserId", UserId);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                User Detail = new User
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Email = reader.GetString(2)

                };
                Userlist.Add(Detail);
            }
            return Userlist;



        }
        public User GetUserWithTasks(int UserId)
        {
            User Detail = null;

            using (SqlConnection connection = new SqlConnection(_connectionStrings))
            {
                connection.Open();

                string query = @"
            SELECT u.UserId, u.UserName, u.Email,
                   t.TaskId, t.Title, t.Description, t.Status, t.CreatedDate
            FROM [User] u
            LEFT JOIN [Task] t ON u.UserId = t.UserId
            WHERE u.UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserId", (SqlDbType)UserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (Detail == null)
                            {
                                Detail = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    UserName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Tasks = new List<TaskItem>()
                                };
                            }

                            if (!reader.IsDBNull(3)) 
                            {
                                Detail.Tasks.Add(new TaskItem
                                {
                                    TaskId = reader.GetInt32(3),
                                    Title = reader.GetString(4),
                                    Description = reader.GetString(5),
                                    Status = reader.GetString(6),
                                    CreatedDate = reader.GetDateTime(7),
                                    UserId = UserId
                                });
                            }
                        }
                    }
                }
            }

            return Detail;
        }

        public User AddUser(string userName, ApiResponse userlist)
        {
            throw new NotImplementedException();
        }

        public User AddUser(string userName, CreateUserDto userlist)
        {
            throw new NotImplementedException();
        }

        public bool EmailExit(string email)
        {
            throw new NotImplementedException();
        }

        public int AddUser(string userName, string email)
        {
            throw new NotImplementedException();
        }
    }
}
