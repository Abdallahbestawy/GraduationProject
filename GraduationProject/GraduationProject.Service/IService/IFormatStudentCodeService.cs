using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.FormatStudentCodeDto;

namespace GraduationProject.Service.IService
{
    public interface IFormatStudentCodeService
    {
        Task<Response<int>> AddFormatStudentCodeAsync(FormatStudentCodeDto addFormatStudentCodeDto);
        Task<Response<int>> UpdateFormatStudentCodeAsync(FormatStudentCodeDto updateFormatStudentCodeDto);
        Task<Response<int>> DeleteFormatStudentCodeAsync(int formatStudentCodeId);
        Task<Response<FormatStudentCodeDto>> GetFormatStudentCodeByIdAsync(int formatStudentCodeId);
        Task<Response<FormatStudentCodeDto>> GetFormatStudentCodeByFacultyIdAsync(int facultyId);
    }
}
