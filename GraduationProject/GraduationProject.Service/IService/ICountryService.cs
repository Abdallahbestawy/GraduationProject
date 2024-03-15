using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CountryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.IService
{
    public interface ICountryService
    {
        Task<Response<List<CountryDto>>> GetAll();
    }
}
