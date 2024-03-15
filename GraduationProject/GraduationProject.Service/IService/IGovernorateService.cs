using GraduationProject.Data.Entity;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.GovernorateDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.IService
{
    public interface IGovernorateService
    {
        Task<Response<List<GovernorateDto>>> GetByCountyId(int countyId);
    }
}
