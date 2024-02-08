using GraduationProject.Identity.Models;

namespace GraduationProject.Identity.IService
{
    public interface IRoleService
    {
        Task<int> AddRole(RoleModel model);
        Task<ICollection<RoleModel>> GetAllRoles();
    }
}
