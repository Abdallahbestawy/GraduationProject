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



        public async Task<List<GetDepartmentDto>> GetDepartmentAllAsync()
        {
            try
            {
                var departments = await _unitOfWork.Departments.GetAll();
                if (departments == null || !departments.Any())
                {
                    return null;
                }

                return departments
                    .Select(department => new GetDepartmentDto
                    {
                        DepartmentId = department.Id,
                        DepartmentName = department.Name
                    })
                    .ToList();
            }
            catch
            {
                return null;
            }
        }

        public async Task<GetDeDepartmentByIdDto> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var dept = await _unitOfWork.Departments
                    .FindWithIncludeIEnumerableAsync(
                        f => f.Faculty
                    );
                dept = dept.Where(d => d.Id == departmentId);


                var department = dept.FirstOrDefault();

                if (department == null)
                {
                    return null;
                }
                var departmentDto = new GetDeDepartmentByIdDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    Description = department.Description,
                    FacultyName = department.Faculty.Name,
                };

                return departmentDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<bool> UpdateDepartmentAsync(DepartmentDto updateDepartmentDto)
        {
            try
            {
                Department existingDepartment = await _unitOfWork.Departments.GetByIdAsync(updateDepartmentDto.Id);
                if (existingDepartment == null)
                {
                    return false;
                }
                existingDepartment.Name = updateDepartmentDto.Name;
                existingDepartment.Description = updateDepartmentDto.Description;
                existingDepartment.Code = updateDepartmentDto.Code;
                existingDepartment.FacultyId = updateDepartmentDto.FacultyId;
                await _unitOfWork.Departments.Update(existingDepartment);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                var oldDept = await _unitOfWork.Departments.GetByIdAsync(departmentId);
                if (oldDept == null)
                {
                    return false;
                }
                await _unitOfWork.Departments.Delete(oldDept);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
