using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.DataTransferObject.FilesDto
{
    public class ValidationErrorsModel
    {
        public int Inedx { get; set; }
        public List<ErrorsList>? Errors { get; set; }
    }

    public class ErrorsList
    {
        public object? ErrorMessage { get; set; }
    }
}
