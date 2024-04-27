using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.DataTransferObject.FilesDto
{
    public class ImportedData<T>
    {
        public List<T> MappedData { get; set; }
        public List<ValidationErrorsModel>? ValidationErrors { get; set; }
    }
}
