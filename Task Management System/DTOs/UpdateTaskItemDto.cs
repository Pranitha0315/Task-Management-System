namespace Task_Management_System.DTOs
{
    public class UpdateTaskItemDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public int UserID { get; set; }
        public int UserId { get; internal set; }
        public int TaskId { get; internal set; }
    }
}
