using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UsersTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
