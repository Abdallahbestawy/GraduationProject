using GraduationProject.Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraduationProject.Data.Entity
{
    public class Faculty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Bylaw> Bylaws { get; set; } = new List<Bylaw>();
        public virtual ICollection<Band> Bands { get; set; } = new List<Band>();
        public virtual ICollection<Phase> Phases { get; set; } = new List<Phase>();
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
        public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();
        public virtual ICollection<ExamRole> ExamRoles { get; set; } = new List<ExamRole>();
        public virtual ICollection<AssessMethod> AssessMethods { get; set; } = new List<AssessMethod>();
    }
}
