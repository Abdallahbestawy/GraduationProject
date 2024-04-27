using GraduationProject.Service.DataTransferObject.FilesDto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.IService
{
    public interface IExcelHelper
    {
        ImportedData<T> Import<T>(string filePath) where T : new();
        string SaveFile(IFormFile file);
    }
}
