using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Identity.Models
{
    public class UserRolesDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RolesDto> Roles { get; set; }
    }
}
