using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ControlService : IControlService
    {

        private readonly UnitOfWork _unitOfWork;
        public ControlService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> RaisingGradesSemesterAsync(int semesterId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesSemesterAsync(semesterId);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task Test(int courseId)
        //{
        //    //var students = await _context.StudentSemesterCourses
        //    //    .Where(crs => crs.CourseId == courseId && crs.StudentSemester.AcademyYear.IsCurrent == true)
        //    //    .Include(sems => sems.StudentSemester)
        //    //    .Include(assess => assess.StudentSemester.StudentSemesterAssessMethods)
        //    //        .ThenInclude(assess=>assess.CourseAssessMethod)
        //    //    .Include(scien => scien.StudentSemester.ScientificDegree)
        //    //        .ThenInclude(bylaw => bylaw.Bylaw)
        //    //            .ThenInclude(est => est.EstimatesCourses).ToListAsync();
        //    var students = await _context.StudentSemesterCourses.
        //        Where(c => c.CourseId == courseId && c.StudentSemester.AcademyYear.IsCurrent)
        //       .Include(assess => assess.StudentSemester.StudentSemesterAssessMethods)
        //         .ThenInclude(assess => assess.CourseAssessMethod).ToListAsync();

        //    foreach (var student in students)
        //    {
        //        decimal? total = 0;
        //        var studentassess = student.StudentSemester.StudentSemesterAssessMethods
        //            .Where(assess => assess.CourseAssessMethod.CourseId == courseId).ToList();
        //        foreach (var assess in studentassess)
        //        {
        //            total += assess.Degree;
        //        }
        //    }
        //}
        //public async Task Test(int courseId)
        //{
        //    var students = await _context.StudentSemesterCourses
        //        .Where(c => c.CourseId == courseId && c.StudentSemester.AcademyYear.IsCurrent)
        //        .Include(assess => assess.StudentSemester.StudentSemesterAssessMethods)
        //        .ThenInclude(assess => assess.CourseAssessMethod)
        //        .ToListAsync();

        //    foreach (var student in students)
        //    {
        //        decimal? total = 0;
        //        //var studentAssessments = student.StudentSemester.StudentSemesterAssessMethods
        //        //    .Where(assess => assess.CourseAssessMethod.CourseId == courseId).ToList();

        //        foreach (var assess in student.StudentSemester.StudentSemesterAssessMethods.)
        //        {
        //            total += assess.Degree;
        //        }
        //    }
        //}
        public async Task<bool> RaisingGradesCourseAsync(int courseId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesCourseAsync(courseId);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }


        }
    }
}
