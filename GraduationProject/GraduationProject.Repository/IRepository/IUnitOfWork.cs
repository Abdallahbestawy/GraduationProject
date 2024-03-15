using GraduationProject.Data.Entity;
using GraduationProject.Data.Models;

namespace GraduationProject.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
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
        IGeneralRepository<Course> Courses { get; }
        IGeneralRepository<Phone> Phones { get; }
        IGeneralRepository<CoursePrerequisite> CoursePrerequisites { get; }
        IGeneralRepository<AssessMethod> AssessMethods { get; }
        IGeneralRepository<AcademyYear> AcademyYears { get; }
        IStudentSemestersRepository StudentSemesters { get; }
        IGeneralRepository<StudentSemesterCourse> StudentSemesterCourses { get; }
        IGeneralRepository<CourseAssessMethod> CourseAssessMethods { get; }
        IGeneralRepository<StaffSemester> StaffSemesters { get; }
        IGeneralRepository<GetStudentDetailsByUserIdModel> GetStudentDetailsByUserIdModels { get; }
        IGeneralRepository<GetStaffDetailsByUserIdModel> GetStaffDetailsByUserIdModels { get; }
        IGeneralRepository<StudentSemesterAssessMethod> StudentSemesterAssessMethods { get; }
        IGeneralRepository<GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel> GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModels { get; }
        IGeneralRepository<GetAllModel> GetAllModels { get; }



        int Save();
        Task<int> SaveAsync();
    }
}
