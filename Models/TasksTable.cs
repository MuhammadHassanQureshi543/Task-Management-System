
namespace TaskManagementSystem.Models
{
    public class TasksTable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; } // Foreign key
        public UsersTable AssignedUser { get; set; } // Navigation property
        public int CreatedBy { get; set; }  // Foreign key
        public UsersTable CreatedUser { get; set; }  // Navigation property
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
