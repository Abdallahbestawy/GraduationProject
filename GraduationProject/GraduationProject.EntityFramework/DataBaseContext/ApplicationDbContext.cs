using GraduationProject.Data.Entity;
using GraduationProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.EntityFramework.DataBaseContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel>()
            .HasNoKey();
            modelBuilder.Entity<GetStudentDetailsByUserIdModel>()
            .HasNoKey();
            modelBuilder.Entity<GetStaffDetailsByUserIdModel>()
                .HasNoKey();
            modelBuilder.Entity<GetAllModel>()
                .HasNoKey();
            modelBuilder.Entity<GetStudentResultModel>()
            .HasNoKey();
            modelBuilder.Entity<GetAllStudentsInSemesterModel>()
            .HasNoKey();
            modelBuilder.Entity<GetStudentsSemesterResultModel>()
           .HasNoKey();
            modelBuilder.Entity<GetStudentInSemesterResultModel>()
          .HasNoKey();
        }

        public DbSet<AcademyYear> AcademyYears { get; set; }
        public DbSet<AssessMethod> AssessMethods { get; set; }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Bylaw> Bylaws { get; set; }
        public DbSet<City> Citys { get; set; }
        public DbSet<Country> Countrys { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAssessMethod> CourseAssessMethods { get; set; }
        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Estimates> Estimates { get; set; }
        public DbSet<EstimatesCourse> EstimatesCourses { get; set; }
        public DbSet<ExamRole> ExamRoles { get; set; }
        public DbSet<Faculty> Facultys { get; set; }
        public DbSet<FamilyData> FamilyDatas { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Phase> phases { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<QualificationData> QualificationDatas { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<ScientificDegree> ScientificDegrees { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffSemester> StaffSemesters { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentSemester> StudentSemesters { get; set; }
        public DbSet<StudentSemesterAssessMethod> StudentSemesterAssessMethods { get; set; }
        public DbSet<StudentSemesterCourse> StudentSemesterCourses { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        // Name Sp
        public DbSet<GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel> SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus { get; set; }

        public DbSet<GetStudentDetailsByUserIdModel> SpGetStudentDetailsByUserId { get; set; }

        public DbSet<GetStaffDetailsByUserIdModel> SpGetStaffDetailsByUserId { get; set; }
        public DbSet<GetAllModel> GetAllStudents { get; set; }
        public DbSet<GetStudentResultModel> GetStudentResult { get; set; }
        public DbSet<GetAllStudentsInSemesterModel> GetAllStudentsInSemester { get; set; }
        public DbSet<GetStudentsSemesterResultModel> GetStudentsSemesterResult { get; set; }
        public DbSet<GetStudentInSemesterResultModel> GetStudentInSemesterResult { get; set; }






    }
}
