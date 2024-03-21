using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.SemesterDto;
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
        public async Task<List<GetAllSemesterCurrentDto>> GetAllSemesterCurrentAsync()
        {
            var semesters = await _unitOfWork.StudentSemesters.GetAllSemesterCurrentAsync();

            if (semesters == null)
            {
                return null;
            }

            var getAllSemesterCurrentDtos = semesters.Select(semester => new GetAllSemesterCurrentDto
            {
                Id = semester.ScientificDegreeId,
                Name = semester.ScientificDegree.Name
            }).ToList();

            return getAllSemesterCurrentDtos;
        }
        public async Task Test()
        {
            await _unitOfWork.StudentSemesters.Test(3);
            //await _unitOfWork.SaveAsync();
        }
    }
}
