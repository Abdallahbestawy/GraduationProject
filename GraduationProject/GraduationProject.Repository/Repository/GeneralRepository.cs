using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;

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

        public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
