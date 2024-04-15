using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Data.Entity
{
    public class ActivityLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string? UserId { get; set; }

        public string? TableName { get; set; }

        public string? RecordId { get; set; }

        public string? Operation { get; set; }

        public string? Event { get; set; }

        public string? OldData { get; set; }

        public string? NewData { get; set; }

        public DateTime LogTime { get; set; }
    }
}
