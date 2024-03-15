using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CityDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.IService
{
    public interface ICityService
    {
        Task<Response<List<CityDto>>> GetCitiesByGovernorateId(int governorateId);
    }
}
