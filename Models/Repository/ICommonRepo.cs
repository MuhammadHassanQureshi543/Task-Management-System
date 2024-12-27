using System.Linq.Expressions;

namespace TaskManagementSystem.Models.Repository
{
    public interface ICommonRepo<T>
    {
        Task<List<T>> getAll();
        Task<List<T>> findAll(Expression<Func<T, bool>> filter);
        Task<T> CreateAsync(T body);
        Task<T> Delte(T body);
        Task<T> getData(Expression<Func<T,bool>> filter);
    }
}
