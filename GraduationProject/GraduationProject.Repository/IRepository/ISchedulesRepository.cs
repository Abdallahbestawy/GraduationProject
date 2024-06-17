using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Repository.IRepository
{
    public interface ISchedulesRepository
    {
        Task<bool> AssignStudentsToSchedule(int ScientificDegree);
    }
}
