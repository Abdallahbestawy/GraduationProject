using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.DepartmentDto;

namespace GraduationProject.Service.IService
{
    public interface IDepartmentService
    {
        Task<Response<int>> AddDepartmentAsync(DepartmentDto departmentDto);
    }
}
