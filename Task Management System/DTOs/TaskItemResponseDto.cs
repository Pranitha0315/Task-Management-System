namespace Task_Management_System.DTOs
{
    public class TaskItemResponseDto
    {
        internal string Status;

        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

    }
}
