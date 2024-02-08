using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Identity.DataBaseContextIdentity;
using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using GraduationProject.Identity.Service;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;
using GraduationProject.Service.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// Register IdentityDbContext with correct DbContextOptions type
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));
// injection
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStaffService, StaffService>();
builder.Services.AddTransient<IAdministrationService, AdministrationService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<ITeacherAssistantService, TeacherAssistantService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IFacultService, FacultService>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddTransient<IRoleService, RoleService>();
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => { })
//    .AddEntityFrameworkStores<IdentityDbContext>();

//builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => { })
//    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequiredLength = 5;
})
.AddEntityFrameworkStores<IdentityDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
