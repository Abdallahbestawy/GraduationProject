using GraduationProject.Data.Entity;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.DepartmentDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly UnitOfWork _unitOfWork;
        public DepartmentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task AddDepartmentAsync(DepartmentDto departmentDto)
        {
            Department newDepartment = new Department
            {
                Name = departmentDto.Name,
                Code = departmentDto.Code,
                Description = departmentDto.Description,
                FacultyId = departmentDto.FacultyId
            };
            await _unitOfWork.Departments.AddAsync(newDepartment);
            _unitOfWork.Save();
        }
    }
}
