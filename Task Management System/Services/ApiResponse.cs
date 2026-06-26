using Task_Management_System.Models;

namespace Task_Management_System.Services
{
    internal class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public User Data { get; set; }
        public List<string> Error { get; internal set; }
    }
}