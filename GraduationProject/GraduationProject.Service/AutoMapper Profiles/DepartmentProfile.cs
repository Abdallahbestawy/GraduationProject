using AutoMapper;
using GraduationProject.Data.Entity;
using GraduationProject.Service.DataTransferObject.DepartmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Service.AutoMapper_Profiles
{
    internal class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department , DepartmentDto>();
            CreateMap<DepartmentDto, Department>();
        }
    }
}
