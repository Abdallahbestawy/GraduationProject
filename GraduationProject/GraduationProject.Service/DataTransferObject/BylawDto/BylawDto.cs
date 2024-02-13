using GraduationProject.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace GraduationProject.Service.DataTransferObject.BylawDto
{
    public class BylawDto
    {
        public int Id { get; set; }
        [Required, MaxLength(1000)]
        public string Name { get; set; }

        public string? Description { get; set; }

        public CourseCategory Type { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }

        public int FacultyId { get; set; }
        //[Required, MaxLength(500)]
        //public string NameEstimates { get; set; }
        //[Required]
        //public char CharEstimates { get; set; }
        //public decimal? MaxPercentageEstimates { get; set; }
        //public decimal? MinPercentageEstimates { get; set; }
        //public decimal? MaxGpaEstimates { get; set; }
        //public decimal? MinGpaEstimates { get; set; }
        //[Required, MaxLength(500)]
        //public string NameEstimatesCourse { get; set; }
        //[Required]
        //public char CharEstimatesCourse { get; set; }
        //public decimal MaxPercentageEstimatesCourse { get; set; }
        //public decimal MinPercentageEstimatesCourse { get; set; }
        public List<EstimateDto> Estimates { get; set; }
        public List<EstimateCourseDto> EstimatesCourses { get; set; }
    }
    public class EstimateDto
    {
        [Required, MaxLength(500)]
        public string NameEstimates { get; set; }
        [Required]
        public char CharEstimates { get; set; }
        public decimal? MaxPercentageEstimates { get; set; }
        public decimal? MinPercentageEstimates { get; set; }
        public decimal? MaxGpaEstimates { get; set; }
        public decimal? MinGpaEstimates { get; set; }
    }

    public class EstimateCourseDto
    {
        [Required, MaxLength(500)]
        public string NameEstimatesCourse { get; set; }
        [Required]
        public char CharEstimatesCourse { get; set; }
        public decimal MaxPercentageEstimatesCourse { get; set; }
        public decimal MinPercentageEstimatesCourse { get; set; }
    }
}
