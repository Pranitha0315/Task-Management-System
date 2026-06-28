namespace Task_Management_System.DTOs
{
    public class CreateTaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
