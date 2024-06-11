using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;
using System.Security.Claims;

namespace GraduationProject.Service.IService
{
    public interface IAdministrationService
    {
        Task<Response<int>> AddAdministrationAsync(AddStaffDto addStaffDto, ClaimsPrincipal user);
        Task<Response<List<GetAllStaffsDto>>> GetAllAdministrationsAsync(int FacultyId);

    }
}
