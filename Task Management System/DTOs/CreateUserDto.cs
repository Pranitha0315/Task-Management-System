namespace Task_Management_System.DTOs
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int UserId { get; internal set; }
    }
}
