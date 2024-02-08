using GraduationProject.Data.Entity;

namespace GraduationProject.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IGeneralRepository<Department> Departments { get; }
        IGeneralRepository<Student> Students { get; }
        IGeneralRepository<Staff> Staffs { get; }
        IGeneralRepository<QualificationData> QualificationDatas { get; }
        IGeneralRepository<FamilyData> FamilyDatas { get; }
        IGeneralRepository<Faculty> Facultys { get; }

        int Save();
    }
}
