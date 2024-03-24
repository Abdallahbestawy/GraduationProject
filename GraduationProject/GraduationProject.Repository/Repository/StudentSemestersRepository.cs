using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Models;
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
                    .Where(sd => sd.Percentage == null && sd.Total == null && sd.TotalCourses == null && sd.AcademyYear.IsCurrent)
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
                    .Include(s => s.ScientificDegree)
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
                    await CalculateStudentCourseDegrees(student, student.ScientificDegree.BylawId);
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
        private async Task CalculateStudentCourseDegrees(StudentSemester student, int bylawId)
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
                course.Char = await CalculateCharEstimatesCourses(total, bylawId);
            }

        }
        private async Task<char?> CalculateCharEstimatesCourses(decimal? coursedegree, int bylawid)
        {
            var dechar = await _context.EstimatesCourses.Where(b => b.BylawId == bylawid).ToListAsync();
            foreach (var d in dechar)
            {
                if (coursedegree <= d.MaxPercentage && coursedegree >= d.MinPercentage)
                {
                    return d.Char;
                }
            }
            return null;
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
                    studentSemester.Char = await CalculateCharEstimates(studentSemester.Percentage, studentSemester.ScientificDegree.BylawId);

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
        private async Task<char?> CalculateCharEstimates(decimal? coursedegree, int bylawid)
        {
            var dechar = await _context.Estimates.Where(b => b.BylawId == bylawid).ToListAsync();
            coursedegree *= 100;
            foreach (var d in dechar)
            {
                if (coursedegree <= d.MaxPercentage && coursedegree >= d.MinPercentage)
                {
                    return d.Char;
                }
            }
            return null;
        }
        public async Task<List<StudentSemester>> EndSemesterAsync(int scientificDegreeId)
        {
            try
            {
                List<StudentSemester> listStudentSemesters = new List<StudentSemester>();
                var studentSemestersComplete = await _context.StudentSemesters
                     .Include(d => d.ScientificDegree)
                     .Include(d => d.AcademyYear)
                     .ToListAsync();

                if (studentSemestersComplete == null || !studentSemestersComplete.Any())
                {
                    return null;
                }
                var studentSemesters = studentSemestersComplete.Where(s => s.ScientificDegreeId == scientificDegreeId && s.AcademyYear.IsCurrent).ToList();
                if (studentSemesters == null || !studentSemesters.Any())
                {
                    return null;
                }
                var studentSemestersOperation = studentSemesters.FirstOrDefault();
                if (studentSemestersOperation == null)
                {
                    return null;
                }
                var studentsem = await GetNextScientificDegrees(studentSemestersOperation);
                if (studentsem == null)
                {
                    return null;
                }
                var studentSemestersNew = studentSemesters.FirstOrDefault();
                // check acdimy year
                int academyYearId;
                DateTime utcNow = DateTime.UtcNow.Date;
                bool flag = utcNow >= studentSemestersOperation.AcademyYear.End;
                if (flag)
                {
                    academyYearId = await AcademyYearOperation(studentSemestersOperation.AcademyYear);
                }
                else
                {
                    academyYearId = studentSemestersOperation.AcademyYearId;
                }
                if (studentsem.Type == 1)
                {
                    foreach (var std in studentSemesters)
                    {
                        StudentSemester newstudentSemester = new StudentSemester();
                        if (studentsem.Percentage != null)
                        {

                            if (std.Passing)
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);

                            }
                            else
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, std.ScientificDegreeId, academyYearId);
                            }
                        }
                        else
                        {
                            newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);
                        }
                        listStudentSemesters.Add(newstudentSemester);
                    }
                }
                else if (studentsem.Type == 2)
                {
                    foreach (var std in studentSemesters)
                    {
                        StudentSemester newstudentSemester = new StudentSemester();
                        if (studentsem.Percentage != null)
                        {
                            var stdDegree = studentSemestersComplete.Where(d => d.StudentId == std.StudentId).ToList();
                            decimal precntage = await RatioCalculation(stdDegree);
                            if (studentsem.Percentage >= precntage)
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);
                            }
                            else
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, std.Id, academyYearId);

                            }
                        }
                        else
                        {
                            newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);
                        }
                        listStudentSemesters.Add(newstudentSemester);
                    }
                }
                else
                {
                    foreach (var std in studentSemesters)
                    {
                        StudentSemester newstudentSemester = new StudentSemester();
                        if (studentsem.Percentage != null)
                        {
                            var stdDegree = studentSemestersComplete.Where(d => d.StudentId == std.StudentId).ToList();
                            decimal precntage = await RatioCalculation(stdDegree);
                            if (studentsem.Percentage >= precntage)
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);
                            }
                            else
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, std.Id, academyYearId);

                            }
                        }
                        else
                        {
                            newstudentSemester = await CreateNewStudentSemester(std, studentsem.Id, academyYearId);
                        }
                        listStudentSemesters.Add(newstudentSemester);
                    }
                }
                return listStudentSemesters;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<StudentSemester> CreateNewStudentSemester(StudentSemester s, int ScientificDegreeId, int academyYearId)
        {
            var newstudentSemester = new StudentSemester
            {
                ScientificDegreeId = ScientificDegreeId,
                AcademyYearId = academyYearId,
                DepartmentId = s.DepartmentId,
                StudentId = s.StudentId
            };
            return newstudentSemester;
        }
        private async Task<decimal> RatioCalculation(List<StudentSemester> studentSemester)
        {
            decimal? total1 = 0;
            decimal? tolal2 = 0;
            foreach (var id in studentSemester)
            {
                total1 += id.Total;
                tolal2 += id.TotalCourses;
            }
            decimal? prec = total1 / tolal2;
            return prec ?? 0;
        }

        public async Task<List<object>> GetTheCurrentSemesterWithStudents()
        {
            var studentsWithoutPercentageAndTotal = await _context.StudentSemesters
            .Where(sd => sd.Percentage == null && sd.Total == null && sd.TotalCourses == null && sd.AcademyYear.IsCurrent)
            .ToListAsync();

            var groupedStudents = studentsWithoutPercentageAndTotal
                .GroupBy(sd => sd.ScientificDegreeId)
                .Select(group => new
                {
                    ScientificDegreeId = group.Key,
                    Students = group.ToList()
                })
                .Cast<object>()
                .ToList();

            return groupedStudents;
        }
        private async Task<GetScientificDegreesNextModel?> GetNextScientificDegrees(StudentSemester studentSemester)
        {
            int Index = studentSemester.ScientificDegree.Order + 1;
            var studentsem = await _context.ScientificDegrees.Where(s => s.Order == Index && s.ParentId == studentSemester.ScientificDegree.ParentId).FirstOrDefaultAsync();
            if (studentsem == null)
            {
                var respone = await GetSemesterByParentId(studentSemester.ScientificDegree);
                return respone ?? null;
            }
            var result = new GetScientificDegreesNextModel
            {
                Id = studentSemester.Id,
                Type = 1,
                Percentage = studentSemester.ScientificDegree.SuccessPercentageSemester
            };
            return result;
        }
        private async Task<GetScientificDegreesNextModel?> GetSemesterByParentId(ScientificDegree scientificDegrees)
        {
            var semesterNext = await _context.ScientificDegrees.Where(s => s.Id == scientificDegrees.ParentId && s.Type == ScientificDegreeType.Band).FirstOrDefaultAsync();
            var Index = semesterNext.Order + 1;
            var studentBand = await _context.ScientificDegrees.Where(s => s.Order == Index && s.Type == ScientificDegreeType.Band).FirstOrDefaultAsync();
            if (studentBand == null)
            {
                var respone = await GetBandByPhaseId(studentBand);
                if (respone == null)
                {
                    return null;
                }
                else
                {
                    var studentsems = await _context.ScientificDegrees.
               Where(s => s.ParentId == respone.Id && s.Type == ScientificDegreeType.Semester).FirstOrDefaultAsync();
                    if (studentsems == null)
                    {
                        return null;
                    }
                    respone.Id = studentsems.Id;
                    return respone;
                }

            }
            var studentsem = await _context.ScientificDegrees.
                Where(s => s.ParentId == studentBand.Id && s.Type == ScientificDegreeType.Semester).FirstOrDefaultAsync();
            if (studentsem == null)
            {
                return null;
            }
            var result = new GetScientificDegreesNextModel
            {
                Id = studentsem.Id,
                Type = 2,
                Percentage = studentsem.SuccessPercentageBand
            };
            return result;
        }
        private async Task<GetScientificDegreesNextModel?> GetBandByPhaseId(ScientificDegree scientificDegrees)
        {
            var phaseNext = await _context.ScientificDegrees.Where(s => s.Id == scientificDegrees.ParentId && s.Type == ScientificDegreeType.Phase).FirstOrDefaultAsync();
            var Index = phaseNext.Order + 1;
            var studentPhase = await _context.ScientificDegrees.Where(s => s.Order == Index && s.Type == ScientificDegreeType.Phase).FirstOrDefaultAsync();
            if (studentPhase == null)
            {
                return null;
            }
            var studentBand = await _context.ScientificDegrees.Where(s => s.ParentId == studentPhase.Id && s.Type == ScientificDegreeType.Band).FirstOrDefaultAsync();
            if (studentBand == null)
            {
                return null;
            }
            var result = new GetScientificDegreesNextModel
            {
                Id = studentBand.Id,
                Type = 3,
                Percentage = studentBand.SuccessPercentagePhase
            };
            return result;

        }

        private async Task<int> AcademyYearOperation(AcademyYear academyYear)
        {
            try
            {
                if (academyYear == null)
                {
                    return -1;
                }
                academyYear.IsCurrent = false;
                DateTime utcNow = DateTime.UtcNow.Date;
                DateTime utcNowNextYear = utcNow.AddYears(1);
                var order = academyYear.AcademyYearOrder + 1;
                AcademyYear newAcademyYear = new AcademyYear
                {
                    IsCurrent = true,
                    AcademyYearOrder = order,
                    Start = utcNow,
                    End = utcNowNextYear,
                    FacultyId = academyYear.FacultyId,
                };
                _context.AcademyYears.Update(academyYear);
                _context.AcademyYears.Add(newAcademyYear);
                int result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return newAcademyYear.Id;
                }
                return -1;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
