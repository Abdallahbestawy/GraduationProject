using GraduationProject.Data.Entity;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Repository.Repository
{
    public class StudentSemestersRepository : GeneralRepository<StudentSemester>, IStudentSemestersRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentSemestersRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task Test(int courseId)
        {
            try
            {
                var students = await _context.StudentSemesters
                              .Include(s => s.StudentSemesterCourse.Where(c => c.CourseId == courseId))
                              .ThenInclude(s => s.Course)
                              .Include(s => s.StudentSemesterAssessMethods.Where(c => c.CourseAssessMethod.CourseId == courseId))
                              .Where(a => a.AcademyYear.IsCurrent)
                              .ToListAsync();
                foreach (var student in students)
                {
                    decimal? total = 0;
                    foreach (var assessMethod in student.StudentSemesterAssessMethods)
                    {
                        total += assessMethod.Degree;
                    }
                    foreach (var course in student.StudentSemesterCourse)
                    {
                        var res = await _context.StudentSemesterCourses.FindAsync(course.Id);
                        res.CourseDegree = total;
                        if (course.Course.MinDegree < total)
                        {
                            res.Passing = true;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private async Task UpdateStudentCourseAsync(StudentSemesterCourse course, decimal total)
        {
            var studentCourse = await _context.StudentSemesterCourses.FindAsync(course.Id);
            if (studentCourse != null)
            {
                studentCourse.CourseDegree = total;
                studentCourse.Passing = course.Course.MinDegree < total;
            }
        }

    }
}
