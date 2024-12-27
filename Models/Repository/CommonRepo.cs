
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Models.Repository
{
    public class CommonRepo<T> : ICommonRepo<T> where T : class
    {
        private readonly DBContext _dbContext;
        private DbSet<T> _dbSet;
        public CommonRepo(DBContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }
        public async Task<T> CreateAsync(T body)
        {
            await _dbSet.AddAsync(body);
            await _dbContext.SaveChangesAsync();
            return body;
        }

        public async Task<T> Delte(T body)
        {
            _dbSet.Remove(body);
            await _dbContext.SaveChangesAsync();
            return body;
        }

        public async Task<List<T>> findAll(Expression<Func<T, bool>> filter)
        {
            var data = await _dbSet.ToListAsync();
            return data;
        }

        public async Task<List<T>> getAll()
        {
            var data = await _dbSet.ToListAsync();
            return data;
        }

        public async Task<T> getData(Expression<Func<T, bool>> filter)
        {
            var data = await _dbSet.FirstOrDefaultAsync(filter);
            return data;
        }
    }
}
