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

        public async Task<bool> RaisingGradesCourseAsync(int courseId)
        {
            try
            {
                var students = await _context.StudentSemesters
                    .Include(s => s.StudentSemesterCourse.Where(c => c.CourseId == courseId))
                        .ThenInclude(s => s.Course)
                    .Include(s => s.StudentSemesterAssessMethods.Where(c => c.CourseAssessMethod.CourseId == courseId))
                        .ThenInclude(assess => assess.CourseAssessMethod)
                    .Where(a => a.AcademyYear.IsCurrent)
                    .ToListAsync();
                if (students == null || !students.Any())
                {
                    return false;
                }

                foreach (var student in students)
                {
                    if (student == null)
                    {
                        continue;
                    }
                    foreach (var course in student.StudentSemesterCourse)
                    {
                        if (course == null)
                        {
                            continue;
                        }
                        decimal? total = 0;

                        foreach (var assessMethod in course.StudentSemester.StudentSemesterAssessMethods)
                        {
                            if (assessMethod == null)
                            {
                                continue;
                            }
                            total += assessMethod.Degree;
                        }
                        course.CourseDegree = total;
                        course.Passing = total > course.Course.MinDegree;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> RaisingGradesSemesterAsync(int SemId)
        {
            try
            {
                var students = await _context.StudentSemesters
                    .Include(s => s.StudentSemesterCourse)
                        .ThenInclude(s => s.Course)
                    .Include(s => s.StudentSemesterAssessMethods)
                        .ThenInclude(assess => assess.CourseAssessMethod)
                    .Where(a => a.AcademyYear.IsCurrent && a.ScientificDegreeId == SemId)
                    .ToListAsync();

                if (students == null || !students.Any())
                {
                    return false;
                }

                foreach (var student in students)
                {
                    CalculateStudentCourseDegrees(student);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void CalculateStudentCourseDegrees(StudentSemester student)
        {
            foreach (var course in student.StudentSemesterCourse)
            {
                if (course == null)
                {
                    continue;
                }

                decimal? total = 0;
                var assessMethods = course.StudentSemester.StudentSemesterAssessMethods
                    .Where(crs => crs.CourseAssessMethod.CourseId == course.CourseId);

                foreach (var assessMethod in assessMethods)
                {
                    if (assessMethod == null)
                    {
                        continue;
                    }
                    total += assessMethod.Degree;
                }

                course.CourseDegree = total;
                course.Passing = total > course.Course.MinDegree;
            }
        }

    }
}
