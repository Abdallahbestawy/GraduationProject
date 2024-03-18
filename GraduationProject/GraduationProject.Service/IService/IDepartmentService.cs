using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.DepartmentDto;

namespace GraduationProject.Service.IService
{
    public interface IDepartmentService
    {
        Task<Response<int>> AddDepartmentAsync(DepartmentDto departmentDto);
        Task<List<GetDepartmentDto>> GetDepartmentAllAsync();
        Task<bool> UpdateDepartmentAsync(DepartmentDto updateDepartmentDto);
        Task<GetDeDepartmentByIdDto> GetDepartmentByIdAsync(int departmentId);
        Task<bool> DeleteDepartmentAsync(int departmentId);

    }
}
