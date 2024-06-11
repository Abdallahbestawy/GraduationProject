namespace GraduationProject.LogHandler.Models
{
    internal class ActivityLogModel
    {
        public string? UserId { get; set; }

        public string? TableName { get; set; }

        public string? RecordId { get; set; }

        public string? Operation { get; set; }

        public string? Event { get; set; }

        public object? OldData { get; set; }

        public object? NewData { get; set; }

        public Type DataType { get; set; }
    }
}
