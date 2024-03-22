using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.DepartmentDto;

namespace GraduationProject.Service.IService
{
    public interface IDepartmentService
    {
        Task<Response<int>> AddDepartmentAsync(DepartmentDto departmentDto);
        Task<Response<List<GetDepartmentDto>>> GetAllDepartmentsAsync();
        Task<Response<bool>> UpdateDepartmentAsync(DepartmentDto updateDepartmentDto);
        Task<Response<GetDeDepartmentByIdDto>> GetDepartmentByIdAsync(int departmentId);
        Task<Response<bool>> DeleteDepartmentAsync(int departmentId);

    }
}
