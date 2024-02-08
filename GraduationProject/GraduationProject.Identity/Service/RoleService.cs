using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Identity.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<int> AddRole(RoleModel model)
        {
            IdentityRole role = new IdentityRole();
            role.Name = model.RoleName;
            IdentityResult result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public async Task<ICollection<RoleModel>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return roles.Select(role => new RoleModel
            {
                Id = role.Id,
                RoleName = role.Name
            }).ToList();
        }
    }
}
