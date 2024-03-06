using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GraduationProject.Repository.Repository
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        public GeneralRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IQueryable<T>> GetAll()
        {
            var entities = await _context.Set<T>().ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Update(T entity)
        {
            var entry = _context.Set<T>().Update(entity);
            return entry.Entity;
        }
        public async Task<IQueryable<T>> FindAllByForeignKeyAsync<TProperty>(Expression<Func<T, TProperty>> foreignKeySelector, TProperty foreignKey)
        {
            var parameter = foreignKeySelector.Parameters.Single();
            var body = Expression.Equal(foreignKeySelector.Body, Expression.Constant(foreignKey));

            var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);

            var entities = await _context.Set<T>().Where(predicate).AsQueryable().ToListAsync();
            return entities.AsQueryable();
        }

        public async Task<IQueryable<T>> FindWithIncludeAsync(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> entitie = _context.Set<T>();
            if (includes != null)
            {
                entitie = includes.Aggregate(entitie, (current, include) => current.Include(include));
            }
            return entitie.AsQueryable();
        }

    }
}
