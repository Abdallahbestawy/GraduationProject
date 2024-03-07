using GraduationProject.Identity.Models;
using GraduationProject.ResponseHandler.Model;

namespace GraduationProject.Identity.IService
{
    public interface IRoleService
    {
        Task<int> AddRole(RoleModel model);
        Task<Response<ICollection<RoleModel>>> GetAllRoles();
    }
}
