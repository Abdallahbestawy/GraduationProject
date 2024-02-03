using GraduationProject.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace GraduationProject.Identity.DataBaseContextIdentity
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    builder.Entity<Staff>()
        //        .HasOne(a => a.ApplicationUser)
        //        .WithOne(b => b.Staff)
        //        .HasForeignKey<Staff>(b => b.UserId);
        //    //base.OnModelCreating(builder);
        //}
    }
}
