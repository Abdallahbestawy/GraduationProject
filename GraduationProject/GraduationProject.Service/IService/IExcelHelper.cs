using GraduationProject.Service.DataTransferObject.FilesDto;
using GraduationProject.Service.DataTransferObject.PhoneDto;
using Microsoft.AspNetCore.Http;

namespace GraduationProject.Service.IService
{
    public interface IExcelHelper
    {
        ImportedData<T> Import<T>(string filePath) where T : new();
        string SaveFile(IFormFile file);
        ImportedData<List<PhoneNumberDto>> ImportFromSpecificColumn(string filePath, string colName);
        Task<MemoryStream> GenerateExcelFileForAssessMethodsAsync(string courseName, string courseCode,
            string lecturerName, string extractorName, List<Dictionary<string, object>> students, List<string> assessMethods);
        List<Dictionary<string, object>> ReadExcelFileForAssessMethods(Stream excelStream);
    }
}
