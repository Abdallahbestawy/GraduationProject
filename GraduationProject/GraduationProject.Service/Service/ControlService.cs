using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Service.Service
{
    public class ControlService : IControlService
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public ControlService(UnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _context = context;
        }
        public async Task Test(int courseId)
        {
            var students = await _context.StudentSemesterCourses
                .Where(crs => crs.CourseId == courseId && crs.StudentSemester.AcademyYear.IsCurrent == true)
                .Include(sems => sems.StudentSemester)
                .Include(assess => assess.StudentSemester.StudentSemesterAssessMethods)
                    .ThenInclude(assess=>assess.CourseAssessMethod)
                .Include(scien => scien.StudentSemester.ScientificDegree)
                    .ThenInclude(bylaw => bylaw.Bylaw)
                        .ThenInclude(est => est.EstimatesCourses).ToListAsync();

            foreach (var student in students)
            {
                decimal? total = 0;
                var studentassess = student.StudentSemester.StudentSemesterAssessMethods
                    .Where(assess => assess.CourseAssessMethod.CourseId == courseId).ToList();
                foreach (var assess in studentassess)
                {
                    total += assess.Degree;
                }
            }
        }
    }
}
