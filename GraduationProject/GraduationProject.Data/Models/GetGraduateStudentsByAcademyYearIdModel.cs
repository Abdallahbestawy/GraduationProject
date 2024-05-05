namespace GraduationProject.Data.Models
{
    public class GetGraduateStudentsByAcademyYearIdModel
    {
        public string AcademyYear { get; set; }
        public string StudentName { get; set; }
        public string StudentCode { get; set; }
        public decimal PercentageTotal { get; set; }
        public char CharTotal { get; set; }
    }
}
