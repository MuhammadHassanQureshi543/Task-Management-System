
using TaskManagementSystem.Models.DTOs;

namespace TaskManagementSystem.Models
{
    public class TaskDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
        public int AssignedTo { get; set; } 
        public int CreatedBy { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AssignedUserName { get; set; }
        public string CreatedUserName { get; set; }
    }
}
