using AutoMapper;
using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _DepartmentMapper;
        private readonly IMailService _mailService;
        public DepartmentService(UnitOfWork unitOfWork, IMapper DepartmentMapper, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _DepartmentMapper = DepartmentMapper;
            _mailService = mailService;
        }
        public async Task<Response<int>> AddDepartmentAsync(DepartmentDto departmentDto)
        {
            //Department newDepartment = new Department
            //{
            //    Name = departmentDto.Name,
            //    Code = departmentDto.Code,
            //    Description = departmentDto.Description,
            //    FacultyId = departmentDto.FacultyId
            //};
            try
            {
                var newDepartment = _DepartmentMapper.Map<Department>(departmentDto);
                await _unitOfWork.Departments.AddAsync(newDepartment);
                var result = await _unitOfWork.SaveAsync();
                if (result > 0)
                    return Response<int>.Created("Department added successfully");

                return Response<int>
                    .ServerError("Error occured while adding Department",
                    "An unexpected error occurred while adding Department. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "DepartmentService",
                    MethodName = "AddDepartmentAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>
                    .ServerError("Error occured while adding Department",
                    "An unexpected error occurred while adding Department. Please try again later.");
            }
        }
    }
}
