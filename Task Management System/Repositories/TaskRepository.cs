using Microsoft.Data.SqlClient;
using Task_Management_System.Models;
using static Task_Management_System.Repositories.TaskRepository;

namespace Task_Management_System.Repositories
{
    public class TaskRepository
    {
            private readonly string _connectionStrings;

            public ProductRepository(IConfiguration configuration)
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
                        TaskItem   Item = new TaskItem
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
        }
        }
    }
}
