namespace GraduationProject.Data.Entity
{
    public class Estimates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char Char { get; set; }
        public double MaxPercentage { get; set; }
        public double MinPercentage { get; set; }
        public double MaxGpa { get; set; }
        public double MinGpa { get; set; }
    }
}
