using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.StaffDto;

namespace GraduationProject.Service.IService
{
    public interface IStaffService
    {
        Task<Response<int>> AddStAffAsync(AddStaffDto addSaffDto);
        Task<Response<int>> AddStaffSemesterAsync(List<AddStaffSemesterDto> addStaffSemesterDto, int type);
        Task<Response<GetCourseStaffSemesterDto>> GetCourseStaffSemesterAsync(string userId);
        Task<Response<GetCourseStaffSemesterDto>> GetCourseStaffSemesterAdministrationAsync(int staffId);
        Task<Response<GetStaffDetailsByUserIdDto>> GetStaffByUserIdAsync(string userId);
        Task<Response<List<GetAllStaffsDto>>> GetAllStaffsAsync(int FacultyId);
        Task<Response<bool>> DeleteStaffSemesterAsync(int staffSemesterId);
        Task<Response<bool>> DeleteAsync(int id);
        Task<Response<int>> UpdateStaffAsync(UpdateStaffDto updateStaffDto);
        Task<Response<GetStaffInfoByStaffIdDto>> GetStaffInfoByStaffIdAsync(int staffId);
        Task<Response<GetDetailsByUserIdDto>> GetDetailsByUserIdAsync(string userId);
        Task<Response<List<GetAllByFacultyIdDto>>> GetAllByFacultyIdAsync(int facultyId);
    }
}
