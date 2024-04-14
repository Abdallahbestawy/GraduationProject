using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.LogHandler.Models
{
    internal class ActivityLogModel<T>
    {
        public string? UserId { get; set; }

        public string? TableName { get; set; }

        public string? RecordId { get; set; }

        public string? Operation { get; set; }

        public string? Event { get; set; }

        public T? OldData { get; set; }

        public T? NewData { get; set; }
    }
}
