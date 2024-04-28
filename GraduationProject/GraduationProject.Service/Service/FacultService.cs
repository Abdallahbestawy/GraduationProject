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

        public async Task<Response<int>> AddFacultAsync(FacultyDto facultyDto, string userId)
        {
            try
            {
                Faculty newFaculty = new Faculty
                {
                    Name = facultyDto.Name,
                    Description = facultyDto.Description,
                    UserId = userId
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

        public async Task<Response<GetFacultyByUserIdDto>> GetFacultByUserIdAsync(string userId)
        {
            try
            {
                var results = await _unitOfWork.Facultys.GetEntityByPropertyAsync(u => u.UserId == userId);

                if (results == null || !results.Any())
                    return Response<GetFacultyByUserIdDto>.NoContent("No faculties are exist");

                var facultys = results.Select(faculty => new GetFacultyDtos
                {
                    FacultId = faculty.Id,
                    FacultName = faculty.Name
                }).ToList();

                var getFacultyByUserIdDto = new GetFacultyByUserIdDto
                {
                    GetFacultyDtos = facultys,
                };

                return Response<GetFacultyByUserIdDto>.Success(getFacultyByUserIdDto, "Faculties retrieved successfully")
                    .WithCount(getFacultyByUserIdDto.GetFacultyDtos.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FacultService",
                    MethodName = "GetFacultByUserIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetFacultyByUserIdDto>.ServerError("Error occured while retrieving faculties",
                    "An unexpected error occurred while retrieving faculties. Please try again later.");
            }
        }

        public async Task<Response<GetFacultyDetailsDto>> GetFacultyDetailsAsync(int facultyId)
        {
            try
            {
                var results = await _unitOfWork.Facultys.FindWithIncludeIEnumerableAsync(
                    x => x.AssessMethods.Where(f => f.FacultyId == facultyId),
                    x => x.Departments.Where(f => f.FacultyId == facultyId),
                    x => x.Bylaws.Where(f => f.FacultyId == facultyId));

                if (results == null || !results.Any())
                    return Response<GetFacultyDetailsDto>.BadRequest("This faculty doesn't exist");

                var facultyBylaws = results.SelectMany(faculty => faculty.Bylaws.Select(bylaw => new FacultyBylawDtos
                {
                    Id = bylaw.Id,
                    BylawName = bylaw.Name
                })).ToList();

                var facultyDepartments = results.SelectMany(faculty => faculty.Departments.Select(dept => new FacultyDepatmentDtos
                {
                    Id = dept.Id,
                    DepatmentName = dept.Name
                })).ToList();

                var facultyAssessMethods = results.SelectMany(faculty => faculty.AssessMethods.Select(ass => new FacultyAssessMethodDtos
                {
                    Id = ass.Id,
                    AssessMethodName = ass.Name
                })).ToList();

                var facultyDetailsDto = new GetFacultyDetailsDto
                {
                    FacultyAssessMethodDtos = facultyAssessMethods,
                    FacultyBylawDtos = facultyBylaws,
                    FacultyDepatmentDtos = facultyDepartments
                };

                return Response<GetFacultyDetailsDto>.Success(facultyDetailsDto, "Faculty's details retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "FacultService",
                    MethodName = "GetFacultyDetailsAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetFacultyDetailsDto>.ServerError("Error occured while retrieving faculty's details",
                    "An unexpected error occurred while retrieving faculty's details. Please try again later.");
            }
        }

    }
}
