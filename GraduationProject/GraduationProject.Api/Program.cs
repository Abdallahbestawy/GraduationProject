using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Identity.DataBaseContextIdentity;
using GraduationProject.Identity.IService;
using GraduationProject.Identity.Models;
using GraduationProject.Identity.Service;
using GraduationProject.Identity.Settings;
using GraduationProject.LogHandler.Service;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Mails.Service;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;
using GraduationProject.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OfficeOpenXml;
using GraduationProject.LogHandler.IService;

var builder = WebApplication.CreateBuilder(args);
JwtTokenLifetimeManager jwtTokenLifetimeManager = new JwtTokenLifetimeManager();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// AutoMapper

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

// Register IdentityDbContext with correct DbContextOptions type
builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddPolicy("Policy", CorsPolicyBuilder =>
    {
        CorsPolicyBuilder
        //.AllowAnyOrigin()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true) // allow any origin
        .AllowAnyMethod()
        .AllowCredentials();
    });
});
// injection
builder.Services.AddTransient<IDepartmentService, DepartmentService>();
builder.Services.AddTransient<IAcademyYearService, AcademyYearService>();
builder.Services.AddTransient<IAssessMethodService, AssessMethodService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStaffService, StaffService>();
builder.Services.AddTransient<IAdministrationService, AdministrationService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<ITeacherAssistantService, TeacherAssistantService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IFacultService, FacultService>();
builder.Services.AddTransient<IBandService, BandService>();
builder.Services.AddTransient<IBylawService, BylawService>();
builder.Services.AddTransient<IExamRoleService, ExamRoleService>();
builder.Services.AddTransient<IPhaseService, PhaseService>();
builder.Services.AddTransient<ISemesterService, SemesterService>();
builder.Services.AddTransient<IScientificDegreeService, ScientificDegreeService>();
builder.Services.AddTransient<IFormatStudentCodeService, FormatStudentCodeService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<UnitOfWork>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<IControlService, ControlService>();
builder.Services.AddTransient<ILocationsService, LocationsService>();
builder.Services.AddTransient<ILoggerHandler, LoggerHandler>();
builder.Services.AddTransient<IExcelHelper, ExcelHelper>();
builder.Services.AddTransient<IJwtTokenLifetimeManager, JwtTokenLifetimeManager>();

builder.Services.AddSingleton<IJwtTokenLifetimeManager>(jwtTokenLifetimeManager);

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

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
.AddDefaultTokenProviders()
.AddEntityFrameworkStores<IdentityDbContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = true;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            LifetimeValidator = jwtTokenLifetimeManager.ValidateTokenLifetime,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Policy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();