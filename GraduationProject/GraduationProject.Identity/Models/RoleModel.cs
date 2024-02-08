using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Identity.Models
{
    public class RoleModel
    {
        public string Id { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
