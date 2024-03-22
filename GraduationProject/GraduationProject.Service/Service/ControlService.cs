using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ControlService : IControlService
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public ControlService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<bool>> RaisingGradesSemesterAsync(int semesterId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesSemesterAsync(semesterId);

                if (!result)
                    return Response<bool>.ServerError("Error occured while raising semester's grades",
                     "An unexpected error occurred while raising semester's grades. Please try again later.");

                await _unitOfWork.SaveAsync();
                return Response<bool>.Success(result, "Raising semester's grades success");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "RaisingGradesSemesterAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while raising semester's grades",
                     "An unexpected error occurred while raising semester's grades. Please try again later.");
            }
        }

        public async Task<Response<bool>> RaisingGradesCourseAsync(int courseId)
        {
            try
            {
                bool result = await _unitOfWork.StudentSemesters.RaisingGradesCourseAsync(courseId);

                if (!result)
                    return Response<bool>.ServerError("Error occured while raising course's grades",
                     "An unexpected error occurred while raising course's grades. Please try again later.");

                await _unitOfWork.SaveAsync();
                return Response<bool>.Success(result, "Raising course's grades success");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "RaisingGradesCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while raising course's grades",
                     "An unexpected error occurred while raising course's grades. Please try again later.");
            }

        }

        public async Task<Response<List<GetAllSemesterCurrentDto>>> GetAllSemesterCurrentAsync()
        {
            try
            {
                var semesters = await _unitOfWork.StudentSemesters.GetAllSemesterCurrentAsync();

                if (semesters == null)
                    return Response<List<GetAllSemesterCurrentDto>>.NoContent("No semesters are exist");

                var getAllSemesterCurrentDtos = semesters.Select(semester => new GetAllSemesterCurrentDto
                {
                    Id = semester.ScientificDegreeId,
                    Name = semester.ScientificDegree.Name
                }).ToList();

                return Response<List<GetAllSemesterCurrentDto>>.Success(getAllSemesterCurrentDtos, "Semesters are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "ControlService",
                    MethodName = "GetAllSemesterCurrentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetAllSemesterCurrentDto>>.ServerError("Error occured while retrieving semesters",
                     "An unexpected error occurred while retrieving semesters. Please try again later.");
            }
        }
        public async Task<bool> EndSemesterAsync(int semesterId)
        {
            try
            {
                var std = await _unitOfWork.StudentSemesters.EndSemesterAsync(semesterId);
                if (std == null)
                {
                    return false;
                }
                await _unitOfWork.StudentSemesters.AddRangeAsync(std);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
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
        public async Task Test()
        {
            await _unitOfWork.StudentSemesters.Test();
        }
    }
}
