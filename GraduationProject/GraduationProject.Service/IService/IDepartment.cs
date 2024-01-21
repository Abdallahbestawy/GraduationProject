using GraduationProject.Data.Entity;

namespace GraduationProject.Service.IService
{
    public interface IDepartment
    {
        Task AddDepartmentAsync(Department entity);
    }
}
