using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Task_Management_System.Models;
using static Task_Management_System.Repositories.TaskRepository;

namespace Task_Management_System.Repositories
{
    public class TaskRepository : ITaskRepository
    {
            private readonly string _connectionStrings;

            public TaskRepository(IConfiguration configuration)
            {
                _connectionStrings = configuration.GetConnectionString("Task_Management");
            }

            public List<TaskItem> GetAllTasks()
            {
                List<TaskItem> TaskItemlist = new List<TaskItem>();



                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = " SELECT  * FROM  Tasks";
                using (SqlCommand command = new SqlCommand(query, connection))

                using (SqlDataReader reader = command.ExecuteReader())

                    while (reader.Read())
                    {
                        TaskItem Item = new TaskItem
                        {
                            TaskId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Status = reader.GetString(3)
                        };
                        TaskItemlist.Add(Item);
                    }


                return TaskItemlist;
            }
            public TaskItem GetTaskById(int TaskId)
            {
                TaskItem Item = null;
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "SELECT  * FROM  Tasks where TaskId=@TaskId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskId", TaskId);
                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Item = new TaskItem()
                        {
                            TaskId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.GetString(2),
                            Status = reader.GetString(3)
                        };

                    }

                }

                return Item;
            }
            public TaskItem SearchTasks(string Title)
            {
                TaskItem Item = null;
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "SELECT  * FROM  Tasks WHERE Title LIKE'T%' OR Title LIKE'%D%' OR Title LIKE'F%' OR Title LIKE'B%' ";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    //command.Parameters.AddWithValue("@Title", Title);
                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Item = new TaskItem()
                        {
                            Title = reader.GetString(1),
                           
                        };

                    }

                }

                return Item;


            }
            public List<TaskItem> AddTask(int TaskId, string Title, string Description, string Status, int UserId) 
            {

                List<TaskItem> TaskItemlist = new List<TaskItem>();
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "INSERT INTO Tasks (TaskID,Title,Description,Status,UserId) VALUES(@TaskID,@Title,@Description,@Status,@UserId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TaskID", TaskId);
                command.Parameters.AddWithValue(" @Title", Title);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    TaskItem Item = new TaskItem
                    {
                        TaskId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Status = reader.GetString(3)
                    };
                    TaskItemlist.Add(Item);
                }
                return TaskItemlist;



            }




            public List<TaskItem> UpdateTask( string Title, string Description, string Status, int UserId)
            {
                List<TaskItem> TaskItemlist = new List<TaskItem>();
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "UPDATE Products SET Title = @Title, Description = @Description,Status = @Status, UserId = @UserId where TaskId= @TaskId";

                using SqlCommand command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@Title", Title);
                command.Parameters.AddWithValue("@Description", Description);
                command.Parameters.AddWithValue("@Status", Status);
                command.Parameters.AddWithValue("@UserId", UserId);
                SqlDataReader reader = command.ExecuteReader();


                while (reader.Read())
                {
                    TaskItem Item = new TaskItem
                    {
                        TaskId = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),
                        Status = reader.GetString(3)
                    };
                    TaskItemlist.Add(Item);
                }
                return TaskItemlist;


            }
            public bool ChangeStatus(string Status, int TaskId)
            {
                TaskItem Item = null;
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "UPDATE Tasks SET Status = @Status where TaskID= @TaskID ";
                using SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.AddWithValue("@TaskId", TaskId);
                command.Parameters.AddWithValue("@Status", Status);
                command.ExecuteNonQuery();
                return true;

            }
            public void DeleteTask(int TaskId)
            {
                using SqlConnection connection = new SqlConnection(_connectionStrings);

                connection.Open();
                string query = "DELETE FROM Tasks WHERE TaskID=@TaskID";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TaskId", TaskId);
                command.ExecuteNonQuery();

            }



        
    }
}
