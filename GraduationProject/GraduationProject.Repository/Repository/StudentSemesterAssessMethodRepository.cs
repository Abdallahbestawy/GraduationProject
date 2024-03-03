using GraduationProject.Data.Entity;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Repository.Repository
{
    public class StudentSemesterAssessMethodRepository : GeneralRepository<StudentSemesterAssessMethod>, IStudentSemesterAssessMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentSemesterAssessMethodRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IQueryable<StudentSemesterAssessMethod>> GetStudentSemesterAssessMethods(int courseId)
        {
            courseId = 1;
            var studentSemesterAssessMethods = await _context.StudentSemesterAssessMethods
     .Include(x => x.CourseAssessMethod)
         .ThenInclude(x => x.AssessMethod)
     .Include(x => x.CourseAssessMethod.Course)
     .Include(x => x.StudentSemester.Student)
         .ThenInclude(x => x.User)
     .Where(x => x != null &&
                 x.CourseAssessMethod != null &&
                 x.CourseAssessMethod.AssessMethod != null &&
                 x.CourseAssessMethod.Course != null &&
                 x.StudentSemester != null &&
                 x.StudentSemester.Student != null &&
                 x.StudentSemester.Student.User != null &&
                 x.CourseAssessMethod.AssessMethod.Id == x.CourseAssessMethodId &&
                 x.CourseAssessMethod.Course.Id == courseId &&
                 x.StudentSemester.Student.Id == x.StudentSemester.StudentId &&
                 x.StudentSemester.Student.UserId == x.StudentSemester.Student.User.Id)
     .ToListAsync();

            return studentSemesterAssessMethods.AsQueryable();
        }
    }
}
