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
        IGeneralRepository<Band> Bands { get; }
        IGeneralRepository<Bylaw> Bylaws { get; }
        IGeneralRepository<Semester> Semesters { get; }
        IGeneralRepository<Phase> Phases { get; }
        IGeneralRepository<ExamRole> ExamRoles { get; }
        IGeneralRepository<ScientificDegree> ScientificDegrees { get; }
        IGeneralRepository<Estimates> Estimates { get; }
        IGeneralRepository<EstimatesCourse> EstimatesCourses { get; }


        int Save();
    }
}
