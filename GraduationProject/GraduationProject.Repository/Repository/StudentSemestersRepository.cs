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

        public async Task<List<StudentSemester>> GetAllSemesterCurrentAsync()
        {
            try
            {
                var semesters = await _context.StudentSemesters
                    .Include(s => s.ScientificDegree)
                    .Where(a => a.AcademyYear.IsCurrent)
                    .GroupBy(a => a.ScientificDegreeId)
                    .Select(g => g.FirstOrDefault())
                    .ToListAsync();
                if (semesters == null || !semesters.Any())
                {
                    return null;
                }
                return semesters;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> RaisingGradesCourseAsync(int courseId)
        {
            try
            {
                var students = await _context.StudentSemesters
                    .Include(s => s.StudentSemesterCourse)
                        .ThenInclude(s => s.Course)
                    .Include(s => s.StudentSemesterAssessMethods)
                        .ThenInclude(assess => assess.CourseAssessMethod)
                    .Where(a => a.AcademyYear.IsCurrent)
                    .ToListAsync();
                if (students == null || !students.Any())
                {
                    return false;
                }

                foreach (var student in students)
                {
                    foreach (var course in student.StudentSemesterCourse.Where(c => c.CourseId == courseId))
                    {
                        decimal? total = course.StudentSemester.StudentSemesterAssessMethods
                            .Where(assess => assess.CourseAssessMethod.CourseId == courseId)
                            .Sum(assessMethod => assessMethod.Degree);

                        course.CourseDegree = total;
                        course.Passing = total > 50;
                    }
                }
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RaisingGradesSemesterAsync(int SemesterId)
        {
            try
            {
                var students = await _context.StudentSemesters
                    .Include(s => s.StudentSemesterCourse)
                        .ThenInclude(s => s.Course)
                    .Include(s => s.StudentSemesterAssessMethods)
                        .ThenInclude(assess => assess.CourseAssessMethod)
                    .Where(a => a.AcademyYear.IsCurrent && a.ScientificDegreeId == SemesterId)
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
                    await CalculateStudentCourseDegrees(student);
                }
                bool result = await CalculateStudentSemesterStatistics(SemesterId);
                if (result)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;

            }
        }
        private async Task CalculateStudentCourseDegrees(StudentSemester student)
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
        private async Task<bool> CalculateStudentSemesterStatistics(int scientificDegreeId)
        {
            try
            {
                var studentSemesters = await _context.StudentSemesters
                    .Include(ss => ss.StudentSemesterCourse)
                        .ThenInclude(s => s.Course)
                    .Include(ss => ss.ScientificDegree)
                    .Where(ss => ss.ScientificDegreeId == scientificDegreeId)
                    .Where(ss => ss.AcademyYear.IsCurrent)
                    .ToListAsync();
                if (studentSemesters == null || !studentSemesters.Any())
                {
                    return false;
                }

                foreach (var studentSemester in studentSemesters)
                {
                    var studentCourses = studentSemester.StudentSemesterCourse.Where(sc => sc.StudentSemesterId == studentSemester.Id).ToList();
                    if (studentCourses == null || !studentCourses.Any())
                    {
                        continue;
                    }

                    decimal? totalMaxDegree = studentCourses.Sum(sc => sc.Course.MaxDegree);
                    decimal? totalCourseDegree = studentCourses.Sum(sc => sc.CourseDegree);

                    studentSemester.Total = totalCourseDegree;
                    studentSemester.Percentage = totalMaxDegree != 0 ? totalCourseDegree / totalMaxDegree : null;

                    if (studentSemester.ScientificDegree.SuccessPercentageSemester.HasValue)
                    {
                        studentSemester.Passing = studentSemester.Percentage > studentSemester.ScientificDegree.SuccessPercentageSemester;
                    }
                    else
                    {
                        studentSemester.Passing = true;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}

