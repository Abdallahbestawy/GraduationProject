using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using GraduationProject.ResponseHandler.Model;
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

        public async Task<Response<ICollection<RoleModel>>> GetAllRoles()
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();

                if (roles.Count == 0)
                    return Response<ICollection<RoleModel>>.NoContent("No roles exists");

                var roleModels = roles.Select(role => new RoleModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                }).ToList();

                return Response<ICollection<RoleModel>>
                    .Success(roleModels, "Roles retrieved successfully");
            }catch (Exception ex)
            {
                return Response<ICollection<RoleModel>>
                    .ServerError("Error occured while feching roles", 
                    "An unexpected error occurred while fetching roles. Please try again later.");
            }
        }
    }
}
