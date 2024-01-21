using GraduationProject.Data.Entity;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;

namespace GraduationProject.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGeneralRepository<Department> Departments { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Departments = new GeneralRepository<Department>(_context);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
