namespace TaskManagementSystem.Models.DTOs
{
    public class TaskResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; }
        public string AssignedUserName { get; set; } // User name
        public int CreatedBy { get; set; }
        public string CreatedUserName { get; set; } // User name
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
