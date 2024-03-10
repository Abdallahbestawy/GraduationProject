using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.FacultyDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class FacultService : IFacultService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        public FacultService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddFacultAsync(FacultyDto facultyDto)
        {
            try
            {
                Faculty newFaculty = new Faculty
                {
                    Name = facultyDto.Name,
                    Description = facultyDto.Description,
                    UserId = "3ed1410b-286c-4064-9193-35b792b8aebf"
                };
                await _unitOfWork.Facultys.AddAsync(newFaculty);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("Faculty added successfully");

                return Response<int>.ServerError("Error occured while adding faculty",
                    "An unexpected error occurred while adding Exam faculty. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FacultService",
                    MethodName = "AddFacultAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding faculty",
                    "An unexpected error occurred while adding faculty. Please try again later.");
            }
        }
    }
}
