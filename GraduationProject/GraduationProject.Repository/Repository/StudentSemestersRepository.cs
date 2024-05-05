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
                    .Include(ac => ac.AcademyYear)
                    .Include(s => s.ScientificDegree)
                        .ThenInclude(parent => parent.Parent)
                    .Where(sd => sd.AcademyYear.IsCurrent)
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
                    .Include(s => s.ScientificDegree)
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
                    int bylawId = student.ScientificDegree.BylawId;
                    foreach (var course in student.StudentSemesterCourse.Where(c => c.CourseId == courseId))
                    {
                        decimal? total = course.StudentSemester.StudentSemesterAssessMethods
                            .Where(assess => assess.CourseAssessMethod.CourseId == courseId)
                            .Sum(assessMethod => assessMethod.Degree);

                        course.CourseDegree = total;
                        course.Passing = total > course.Course.MinDegree;
                        course.Char = await CalculateCharEstimatesCourses(total, bylawId);
                    }
                }
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
                    .Where(a => a.ScientificDegreeId == SemesterId)
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
                    studentSemester.TotalCourses = totalMaxDegree;
                    Result result = new Result();
                    result.StudentSemesterId = studentSemester.Id;
                    result.Percentage = totalCourseDegree / totalMaxDegree;
                    result.Char = await CalculateCharEstimates(result.Percentage, studentSemester.ScientificDegree.BylawId);
                    result.PercentageTotal = await RatioCalculation(studentSemester.StudentId);
                    result.CharTotal = await CalculateCharEstimates(result.PercentageTotal, studentSemester.ScientificDegree.BylawId);
                    _context.Results.Add(result);
                    if (studentSemester.ScientificDegree.SuccessPercentageSemester.HasValue)
                    {
                        studentSemester.Passing = result.Percentage > studentSemester.ScientificDegree.SuccessPercentageSemester;
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
        public async Task<(List<StudentSemester>, List<StudentSemester>, bool)> EndSemesterAsync(int scientificDegreeId)
        {
            try
            {
                List<StudentSemester> listStudentSemesters = new List<StudentSemester>();
                List<StudentSemester> updatelistStudentSemesters = new List<StudentSemester>();
                var studentSemestersComplete = await _context.StudentSemesters
                     .Include(d => d.ScientificDegree)
                     .Include(d => d.AcademyYear)
                     .Include(d => d.Results)
                     .ToListAsync();

                if (studentSemestersComplete == null || !studentSemestersComplete.Any())
                {
                    return (null, null, false);

                }
                var studentSemesters = studentSemestersComplete.Where(s => s.ScientificDegreeId == scientificDegreeId && s.AcademyYear.IsCurrent).ToList();
                if (studentSemesters == null || !studentSemesters.Any())
                {
                    return (null, null, false);
                }
                var studentSemestersOperation = studentSemesters.FirstOrDefault();
                if (studentSemestersOperation == null)
                {
                    return (null, null, false);
                }
                var studentSemestersNew = studentSemesters.FirstOrDefault();
                // check acdimy year
                var academyYear = await _context.AcademyYears.OrderByDescending(o => o.AcademyYearOrder).FirstOrDefaultAsync();
                int academyYearId = academyYear.Id;
                var studentsem = await GetNextScientificDegrees(studentSemestersOperation);
                if (studentsem.Type == 4)
                {
                    foreach (var std in studentSemesters)
                    {
                        StudentSemester updateStudentSemester = new StudentSemester();
                        StudentSemester newstudentSemester = new StudentSemester();
                        if (studentsem.Percentage != null)
                        {

                            if (std.Passing)
                            {
                                bool graduate = await IsGraduate(std);
                                if (graduate)
                                {
                                    updateStudentSemester = await UpdateStudentSemester(std);
                                    updatelistStudentSemesters.Add(updateStudentSemester);
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, std.ScientificDegreeId, academyYearId);
                                listStudentSemesters.Add(newstudentSemester);
                            }
                        }
                        else
                        {
                            bool graduate = await IsGraduate(std);
                            if (graduate)
                            {
                                updateStudentSemester = await UpdateStudentSemester(std);
                                updatelistStudentSemesters.Add(updateStudentSemester);
                            }
                            else
                            {
                                newstudentSemester = await CreateNewStudentSemester(std, std.ScientificDegreeId, academyYearId);
                                listStudentSemesters.Add(newstudentSemester);
                            }
                        }

                    }
                    return (listStudentSemesters, updatelistStudentSemesters, true);
                }
                else if (studentsem.Type == 1)
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
                            var precntage = std.Results.Where(sd => sd.StudentSemesterId == std.Id).FirstOrDefault();
                            if (precntage == null)
                            {
                                continue;
                            }
                            if (studentsem.Percentage <= precntage.PercentageTotal)
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
                            var precntage = std.Results.Where(sd => sd.StudentSemesterId == std.Id).FirstOrDefault();
                            if (precntage == null)
                            {
                                continue;
                            }
                            if (studentsem.Percentage <= precntage.PercentageTotal)
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
                return (listStudentSemesters, null, false);
            }
            catch (Exception ex)
            {
                return (null, null, false);
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
        private async Task<StudentSemester> UpdateStudentSemester(StudentSemester s)
        {
            var updatestudentSemester = new StudentSemester
            {
                Id = s.Id,
                ScientificDegreeId = s.ScientificDegreeId,
                AcademyYearId = s.AcademyYearId,
                DepartmentId = s.DepartmentId,
                StudentId = s.StudentId,
                Passing = s.Passing,
                Total = s.Total,
                TotalCourses = s.TotalCourses,
                IsGraduate = true
            };
            return updatestudentSemester;
        }
        private async Task<bool> IsGraduate(StudentSemester s)
        {
            var graduateValuerRequired = await _context.Bylaws.Where(d => d.Id == s.ScientificDegree.BylawId).FirstOrDefaultAsync();
            int Totalearnedpointa = await CalculationTotalearnedpointa(s.StudentId);
            if (graduateValuerRequired.GraduateValuerRequired == Totalearnedpointa)
            {
                return true;
            }
            return false;
        }
        private async Task<int> CalculationTotalearnedpointa(int studentId)
        {
            int totalPoints = 0;
            var studentSemAll = await _context.StudentSemesters.Where(s => s.StudentId == studentId).ToListAsync();
            foreach (var std in studentSemAll)
            {
                var studentCourse = await _context.StudentSemesterCourses.Include(c => c.Course).Where(s => s.StudentSemesterId == std.Id && s.Passing).ToListAsync();
                foreach (var po in studentCourse)
                {
                    totalPoints += po.Course.NumberOfCreditHours ?? 0;
                }
            }
            return totalPoints;
        }

        private async Task<decimal?> RatioCalculation(int studentId)
        {
            var studentSemester = await _context.StudentSemesters.Where(sd => sd.StudentId == studentId).ToListAsync();
            if (studentSemester == null || !studentSemester.Any())
            {
                return null;
            }
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
            .Where(sd => sd.Total == null && sd.TotalCourses == null && sd.AcademyYear.IsCurrent)
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
            GetScientificDegreesNextModel result = new GetScientificDegreesNextModel();
            if (studentsem == null)
            {
                var respone = await GetSemesterByParentId(studentSemester.ScientificDegree);
                if (respone == null)
                {
                    result.Id = 0;
                    result.Type = 4;
                    result.Percentage = studentSemester.ScientificDegree.SuccessPercentageSemester;
                    return result;
                }
                return respone;
            }
            result.Id = studentsem.Id;
            result.Type = 1;
            result.Percentage = studentsem.SuccessPercentageSemester;
            return result;
        }
        private async Task<GetScientificDegreesNextModel?> GetSemesterByParentId(ScientificDegree scientificDegrees)
        {
            var semesterNext = await _context.ScientificDegrees.Where(s => s.Id == scientificDegrees.ParentId && s.Type == ScientificDegreeType.Band).FirstOrDefaultAsync();
            var Index = semesterNext.Order + 1;
            var studentBand = await _context.ScientificDegrees.Where(s => s.Order == Index && s.Type == ScientificDegreeType.Band).FirstOrDefaultAsync();
            if (studentBand == null)
            {
                var respone = await GetBandByPhaseId(semesterNext);
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
                Percentage = studentBand.SuccessPercentageBand
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
                Percentage = studentPhase.SuccessPercentagePhase
            };
            return result;

        }

        public async Task<List<StudentSemester>> GetAllSemesterActiveAsync(int academyYearId)
        {
            try
            {
                var semester = await _context.StudentSemesters
                 .Include(a => a.AcademyYear)
                .Include(s => s.ScientificDegree)
                    .ThenInclude(parent => parent.Parent)
               .Where(a => a.AcademyYearId == academyYearId)
                .GroupBy(d => d.ScientificDegreeId)
                .Select(g => g.First())
                .ToListAsync();
                if (semester == null || !semester.Any())
                {
                    return null;
                }
                return semester;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<List<StudentSemester>> GetAllAcdemyYearGraduatesAsync()
        {
            try
            {
                var semester = await _context.StudentSemesters
                 .Include(a => a.AcademyYear)
               .Where(a => a.IsGraduate)
                .GroupBy(d => d.AcademyYearId)
                .Select(g => g.First())
                .ToListAsync();
                if (semester == null || !semester.Any())
                {
                    return null;
                }
                return semester;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
