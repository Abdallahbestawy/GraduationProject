using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Identity.IService
{
    public interface IJwtTokenLifetimeManager
    {
        void SignOut(JwtSecurityToken jwtSecurityToken);
    }
}
