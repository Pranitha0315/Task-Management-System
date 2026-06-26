

namespace Task_Management_System.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<TaskItem> Tasks { get; internal set; }
        public int Count { get; internal set; }

        public static implicit operator User(TaskItem v)
        {
            throw new NotImplementedException();
        }
    }
}
