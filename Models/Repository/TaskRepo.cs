namespace TaskManagementSystem.Models.Repository
{
    public class TaskRepo : CommonRepo<TasksTable>, ITaskRepo
    {
        public TaskRepo(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
