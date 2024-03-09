using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.ResponseHandler.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Identity.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        public RoleService(RoleManager<IdentityRole> roleManager, IMailService mailService)
        {
            _roleManager = roleManager;
            _mailService = mailService;
        }

        public async Task<Response<int>> AddRole(RoleModel model)
        {
            try
            {
                IdentityRole role = new IdentityRole();
                role.Name = model.RoleName;
                IdentityResult result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                    return Response<int>.Success(1, "Role added successfully");
                else
                    return Response<int>.ServerError("Error occured while adding role", result.Errors);
            }
            catch (Exception ex)
            {
                return Response<int>.ServerError("Error occured while adding role",
                    "An unexpected error occurred while adding role. Please try again later.");
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
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "RoleService",
                    MethodName = "GetAllRoles",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });

                return Response<ICollection<RoleModel>>
                    .ServerError("Error occured while feching roles", 
                    "An unexpected error occurred while fetching roles. Please try again later.");
            }
        }
    }
}
