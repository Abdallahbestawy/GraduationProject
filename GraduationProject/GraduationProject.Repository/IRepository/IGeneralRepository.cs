using System.Linq.Expressions;

namespace GraduationProject.Repository.IRepository
{
    public interface IGeneralRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IQueryable<T>> GetAll();
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<IQueryable<T>> FindAllByForeignKeyAsync<TProperty>(Expression<Func<T, TProperty>> foreignKeySelector, TProperty foreignKey);
    }
}
