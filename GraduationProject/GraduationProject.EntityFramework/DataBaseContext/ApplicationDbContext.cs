using GraduationProject.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.EntityFramework.DataBaseContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Department> Departments { get; set; }
    }
}
