using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.FilesDto
{
    public class UploadExcelFileDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
