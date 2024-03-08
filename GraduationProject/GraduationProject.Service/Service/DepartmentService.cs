using AutoMapper;
using GraduationProject.Data.Entity;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _DepartmentMapper;
        public DepartmentService(UnitOfWork unitOfWork, IMapper DepartmentMapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _DepartmentMapper = DepartmentMapper;
        }
        public async Task AddDepartmentAsync(DepartmentDto departmentDto)
        {
            //Department newDepartment = new Department
            //{
            //    Name = departmentDto.Name,
            //    Code = departmentDto.Code,
            //    Description = departmentDto.Description,
            //    FacultyId = departmentDto.FacultyId
            //};
            var newDepartment = _DepartmentMapper.Map<Department>(departmentDto);
            await _unitOfWork.Departments.AddAsync(newDepartment);
            await _unitOfWork.SaveAsync();
        }
    }
}
