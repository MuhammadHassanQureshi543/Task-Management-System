namespace TaskManagementSystem.Models.Repository
{
    public class UserRepo : CommonRepo<UsersTable>, IUserRepo
    {
        public UserRepo(DBContext dbContext) : base(dbContext)
        {
        }
    }
}
