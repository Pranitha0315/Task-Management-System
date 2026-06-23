namespace Task_Management_System.DTOs
{
    public class ChangeStatusDto
    {
        public string? Status { get; set; }
        public int TaskId { get; internal set; }
    }
}
