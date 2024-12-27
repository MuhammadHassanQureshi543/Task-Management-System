using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models.Config;

namespace TaskManagementSystem.Models
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options): base(options){ }


        public DbSet<UsersTable> Users { get; set; }
        public DbSet<TasksTable> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new TaskConfig());
        }
    }
}
