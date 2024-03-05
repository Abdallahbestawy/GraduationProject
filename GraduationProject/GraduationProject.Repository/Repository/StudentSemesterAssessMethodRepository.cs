using GraduationProject.Data.Entity;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Repository.Repository
{
    public class StudentSemesterAssessMethodRepository : GeneralRepository<StudentSemesterAssessMethod>, IStudentSemesterAssessMethodRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentSemesterAssessMethodRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IQueryable<AssessMethod>> GetStudentSemesterAssessMethods(int courseId)
        {
            //var StudentSemesterCourses = await _context.StudentSemesterCourses.Where(crs=>crs.CourseId == courseId)
            //    .Include(stdsems=>stdsems.StudentSemester).ThenInclude(std=>std.Student)
            //    .Include(stdsems=>stdsems.StudentSemester.StudentSemesterAssessMethods)
            //    .ThenInclude(stdsems=>stdsems.CourseAssessMethod)
            //    .ThenInclude(stdsems=>stdsems.AssessMethod)
            //    .Where(std=>std.StudentSemester.AcademyYear.IsCurrent == true).ToListAsync();

            //var students = StudentSemesterCourses.Select(std => std.StudentSemester.Student).ToList();


            //var assessmethods = StudentSemesterCourses
            //    .SelectMany(assess => assess.StudentSemester?.StudentSemesterAssessMethods ?? Enumerable.Empty<StudentSemesterAssessMethod>())
            //    .Where(assess => assess.CourseAssessMethod != null && assess.CourseAssessMethod.CourseId == courseId)
            //    .Select(assess => assess.CourseAssessMethod.AssessMethod).Distinct()
            //    .ToList();



            //var studentsDegrees = new List<StudentsAssessMethodDegrees>();
            //foreach (var student in students)
            //{
            //    var studentSemesterAssessMethods = StudentSemesterCourses.Where(std => std.StudentSemester.Student.Id == student.Id)
            //        .Select(assess => assess.StudentSemester.StudentSemesterAssessMethods).SingleOrDefault();


            //    var studentDegrees = new StudentsAssessMethodDegrees();
            //    var degrees = new List<AssessMethodDTO>();
            //    foreach (var assess in assessmethods)
            //    {
            //        var degree = new AssessMethodDTO();
            //        degree.AssessMethodId = assess.Id;
            //        degree.Degree = studentSemesterAssessMethods.Where(crs => crs.CourseAssessMethod.CourseId == courseId
            //        && crs.CourseAssessMethod.AssessMethodId == assess.Id).Select(deg => deg.Degree).FirstOrDefault();
            //        degrees.Add(degree);
            //    }
            //    studentDegrees.StdId = student.Id;
            //    studentDegrees.Degrees = degrees;
            //    studentsDegrees.Add(studentDegrees);
            //}

            //var studentsDegrees = await _context.StudentSemesterCourses
            //        .Where(crs => crs.CourseId == courseId)
            //        .Include(stdsems => stdsems.StudentSemester)
            //            .ThenInclude(std => std.Student)
            //                .ThenInclude(student => student.User) // Include Student User
            //        .Include(stdsems => stdsems.StudentSemester.StudentSemesterAssessMethods)
            //            .ThenInclude(stdsems => stdsems.CourseAssessMethod)
            //        .Where(std => std.StudentSemester.AcademyYear.IsCurrent)
            //        .SelectMany(crs => crs.StudentSemester.StudentSemesterAssessMethods)
            //        .Where(assess => assess.CourseAssessMethod != null && assess.CourseAssessMethod.CourseId == courseId)
            //        .Select(assess => new
            //        {
            //            StudentId = assess.StudentSemester.StudentId,
            //            StudentName = assess.StudentSemester.Student.User.NameArabic, // Access Student Name
            //            CourseName = assess.CourseAssessMethod.Course.Name, // Access Course Name
            //            CourseCode = assess.CourseAssessMethod.Course.Code, // Access Course Code
            //            AssessMethodName = assess.CourseAssessMethod.AssessMethod.Name, // Access AssessMethod Name
            //            AssessMethodId = assess.CourseAssessMethod.AssessMethodId,
            //            Degree = assess.Degree
            //        })
            //        .GroupBy(x => x.StudentId)
            //        .Select(group => new StudentsAssessMethodDegrees
            //        {
            //            StdId = group.Key,
            //            Degrees = group.Select(assess => new AssessMethodDTO
            //            {
            //                AssessMethodId = assess.AssessMethodId,
            //                Degree = assess.Degree
            //            }).ToList()
            //        })
            //        .ToListAsync();


            IQueryable<AssessMethod> assessmethodss = Enumerable.Empty<AssessMethod>().AsQueryable();

            return assessmethodss;
        }
    }

    class StudentsAssessMethodDegrees
    {
        public int StdId { get; set; }
        public List<AssessMethodDTO> Degrees { get; set; }
    }
    class AssessMethodDTO
    {
        public int AssessMethodId { get; set; }
        public decimal Degree { get; set; }
    }

}
