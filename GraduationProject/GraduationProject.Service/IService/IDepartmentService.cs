using GraduationProject.Service.DataTransferObject.DepartmentDto;

namespace GraduationProject.Service.IService
{
    public interface IDepartmentService
    {
        Task AddDepartmentAsync(DepartmentDto departmentDto);
    }
}
