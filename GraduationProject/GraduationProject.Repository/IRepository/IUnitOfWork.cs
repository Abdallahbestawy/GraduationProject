using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IGeneralRepository<Department> Departments { get; }
        int Save();
    }
}
