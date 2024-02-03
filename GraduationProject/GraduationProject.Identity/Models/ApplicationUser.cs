using GraduationProject.Data.Entity;
using Microsoft.AspNetCore.Identity;


namespace GraduationProject.Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Staff? Staff { get; set; }
    }
}
