using Microsoft.Data.SqlClient;
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
        Task<IEnumerable<T>> GetEntityByPropertyAsync(Func<T, bool> predicate);
        Task<IQueryable<T>> FindWithIncludeIQueryableAsync(params Expression<Func<T, object>>[] includes);
        Task<IQueryable<T>> FindAllByForeignKeyAsync<TProperty>(Expression<Func<T, TProperty>> foreignKeySelector, TProperty foreignKey);
        Task<IEnumerable<T>> CallStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters);
        Task<IEnumerable<T>> FindWithIncludeIEnumerableAsync(params Expression<Func<T, object>>[] includes);


    }
}
