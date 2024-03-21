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
                    studentSemester.TotalCourses = totalMaxDegree;

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
        public async Task Test(int scientificDegreeId)
        {
            List<StudentSemester> studentSemesters = new List<StudentSemester>();
            var stdCompl = await _context.StudentSemesters.Include(d => d.ScientificDegree).Where(ss => /*ss.ScientificDegreeId == scientificDegreeId &&*/
            ss.AcademyYear.IsCurrent).ToListAsync();
            var std = stdCompl.Where(s => s.ScientificDegreeId == scientificDegreeId);
            var Index = std.FirstOrDefault().ScientificDegree.Order;
            Index++;
            var stdsem = await _context.ScientificDegrees.Where(s => s.Order == Index).FirstOrDefaultAsync();
            var sd = std.FirstOrDefault();
            if (sd.ScientificDegree.ParentId == stdsem.ParentId)
            {
                foreach (var s in std)
                    if (s.ScientificDegree.ParentId == stdsem.ParentId)
                    {
                        StudentSemester newstudentSemester = new StudentSemester();

                        if (s.Passing)
                        {
                            {
                                newstudentSemester.ScientificDegreeId = stdsem.Id;
                                newstudentSemester.AcademyYearId = s.AcademyYearId;
                                newstudentSemester.DepartmentId = s.DepartmentId;
                                newstudentSemester.StudentId = s.StudentId;
                            };
                        }
                        else
                        {
                            newstudentSemester.ScientificDegreeId = s.ScientificDegreeId;
                            newstudentSemester.AcademyYearId = s.AcademyYearId;
                            newstudentSemester.DepartmentId = s.DepartmentId;
                            newstudentSemester.StudentId = s.StudentId;
                        }
                        studentSemesters.Add(newstudentSemester);
                    }
            }
            else
            {
                var parent1 = await _context.ScientificDegrees.Where(sh => sh.Id == sd.ScientificDegree.ParentId).FirstOrDefaultAsync();
                var parent2 = await _context.ScientificDegrees.Where(sh => sh.Id == stdsem.ParentId).FirstOrDefaultAsync();
                if (parent1.ParentId == parent2.ParentId)
                {
                    foreach (var sn in std)
                    {
                        //if (parent2.SuccessPercentageBand != null)
                        // {
                        decimal? total = 0;
                        var stdTest = stdCompl.Where(d => d.StudentId == sn.StudentId).ToList();
                        decimal? total1 = 0;
                        decimal? tolal2 = 0;
                        foreach (var id in stdTest)
                        {
                            total1 = id.Total;
                            tolal2 = id.TotalCourses;
                        }
                        //}
                    }
                }


                await _context.AddRangeAsync(studentSemesters);
            }
        }


    }
}
