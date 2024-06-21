using GraduationProject.Data.Entity;
using GraduationProject.Data.Models;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;

namespace GraduationProject.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGeneralRepository<Department> Departments { get; private set; }
        public IGeneralRepository<Student> Students { get; private set; }
        public IGeneralRepository<Staff> Staffs { get; private set; }
        public IGeneralRepository<QualificationData> QualificationDatas { get; private set; }
        public IGeneralRepository<FamilyData> FamilyDatas { get; private set; }
        public IGeneralRepository<Faculty> Facultys { get; private set; }
        public IGeneralRepository<Band> Bands { get; private set; }
        public IGeneralRepository<Bylaw> Bylaws { get; private set; }
        public IGeneralRepository<Semester> Semesters { get; private set; }
        public IGeneralRepository<FormatStudentCode> FormatStudentCodes { get; private set; }
        public IGeneralRepository<Phase> Phases { get; private set; }
        public IGeneralRepository<ExamRole> ExamRoles { get; private set; }
        public IGeneralRepository<ScientificDegree> ScientificDegrees { get; private set; }
        public IGeneralRepository<Estimates> Estimates { get; private set; }
        public IGeneralRepository<EstimatesCourse> EstimatesCourses { get; private set; }
        public IGeneralRepository<Course> Courses { get; private set; }
        public IGeneralRepository<Phone> Phones { get; private set; }
        public IGeneralRepository<CoursePrerequisite> CoursePrerequisites { get; private set; }
        public IGeneralRepository<AssessMethod> AssessMethods { get; private set; }
        public IGeneralRepository<Schedule> Schedules { get; private set; }
        public IGeneralRepository<AcademyYear> AcademyYears { get; private set; }
        public IGeneralRepository<SchedulePlace> SchedulePlaces { get; private set; }
        public IStudentSemestersRepository StudentSemesters { get; private set; }
        public IGeneralRepository<StudentSemesterCourse> StudentSemesterCourses { get; private set; }
        public IGeneralRepository<CourseAssessMethod> CourseAssessMethods { get; private set; }
        public IGeneralRepository<StudentSemesterAssessMethod> StudentSemesterAssessMethods { get; private set; }

        public IGeneralRepository<StaffSemester> StaffSemesters { get; private set; }
        public IGeneralRepository<GetStudentDetailsByUserIdModel> GetStudentDetailsByUserIdModels { get; private set; }
        public IGeneralRepository<GetStudentScheduleByUserIdModel> GetStudentScheduleByUserIdModels { get; private set; }
        public IGeneralRepository<GetStaffDetailsByUserIdModel> GetStaffDetailsByUserIdModels { get; private set; }
        public IGeneralRepository<GetStudentResultModel> GetStudentResultModels { get; private set; }
        public IGeneralRepository<GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel> GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModels { get; private set; }
        public IGeneralRepository<GetAllModel> GetAllModels { get; private set; }
        public IGeneralRepository<GetAllStudentsInSemesterModel> GetAllStudentsInSemesterModels { get; private set; }
        public IGeneralRepository<GetStudentsSemesterResultModel> GetStudentsSemesterResultModels { get; private set; }
        public IGeneralRepository<GetStudentInSemesterResultModel> GetStudentInSemesterResultModels { get; private set; }
        public IGeneralRepository<GetSchedulesForStaffByUserIdModel> GetSchedulesForStaffByUserIdModels { get; private set; }
        public IGeneralRepository<GetAllStudentInCourseResultModel> GetAllStudentInCourseResultModels { get; private set; }
        public IGeneralRepository<GetAllByFacultyIdModel> GetAllByFacultyIdModels { get; private set; }
        public IGeneralRepository<GetStudentInfoByStudentIdModel> GetStudentInfoByStudentIdModels { get; private set; }
        public IGeneralRepository<GetStudentCourseInfoModel> GetStudentCourseInfoModels { get; private set; }
        public IGeneralRepository<GetStudentBySectionIdModel> GetStudentBySectionIdModels { get; private set; }
        public IGeneralRepository<GetStaffInfoByStaffIdModel> GetStaffInfoByStaffIdModels { get; private set; }
        public IGeneralRepository<GetGraduateStudentsByAcademyYearIdModel> GetGraduateStudentsByAcademyYearIdModels { get; private set; }
        public IGeneralRepository<GetCurrentStaffByCourseIdModel> GetCurrentStaffByCourseIdModels { get; private set; }
        public IGeneralRepository<GetScheduleDetailsModel> GetScheduleDetailsModels { get; private set; }

        public IGeneralRepository<City> Cities { get; private set; }
        public IGeneralRepository<Country> Countries { get; private set; }
        public IGeneralRepository<Governorate> Governorates { get; private set; }
        public IGeneralRepository<ActivityLog> ActivityLogs { get; set; }
        public IGeneralRepository<Result> Results { get; private set; }
        public ISchedulesRepository SchedulesRepo { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Departments = new GeneralRepository<Department>(_context);
            Students = new GeneralRepository<Student>(_context);
            QualificationDatas = new GeneralRepository<QualificationData>(_context);
            FamilyDatas = new GeneralRepository<FamilyData>(_context);
            Staffs = new GeneralRepository<Staff>(_context);
            Facultys = new GeneralRepository<Faculty>(_context);
            Bands = new GeneralRepository<Band>(_context);
            Bylaws = new GeneralRepository<Bylaw>(_context);
            Semesters = new GeneralRepository<Semester>(_context);
            Phases = new GeneralRepository<Phase>(_context);
            ExamRoles = new GeneralRepository<ExamRole>(_context);
            ScientificDegrees = new GeneralRepository<ScientificDegree>(_context);
            Estimates = new GeneralRepository<Estimates>(_context);
            EstimatesCourses = new GeneralRepository<EstimatesCourse>(_context);
            Courses = new GeneralRepository<Course>(_context);
            Schedules = new GeneralRepository<Schedule>(_context);
            SchedulePlaces = new GeneralRepository<SchedulePlace>(_context);
            Phones = new GeneralRepository<Phone>(_context);
            CoursePrerequisites = new GeneralRepository<CoursePrerequisite>(_context);
            AssessMethods = new GeneralRepository<AssessMethod>(_context);
            AcademyYears = new GeneralRepository<AcademyYear>(_context);
            StudentSemesters = new StudentSemestersRepository(_context);
            StudentSemesterCourses = new GeneralRepository<StudentSemesterCourse>(_context);
            CourseAssessMethods = new GeneralRepository<CourseAssessMethod>(_context);
            StudentSemesterAssessMethods = new GeneralRepository<StudentSemesterAssessMethod>(_context);
            StaffSemesters = new GeneralRepository<StaffSemester>(_context);
            FormatStudentCodes = new GeneralRepository<FormatStudentCode>(_context);
            GetStudentDetailsByUserIdModels = new GeneralRepository<GetStudentDetailsByUserIdModel>(_context);
            GetStaffDetailsByUserIdModels = new GeneralRepository<GetStaffDetailsByUserIdModel>(_context);
            GetStudentResultModels = new GeneralRepository<GetStudentResultModel>(_context);
            GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModels = new GeneralRepository<GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModel>(_context);
            GetAllModels = new GeneralRepository<GetAllModel>(_context);
            GetAllStudentsInSemesterModels = new GeneralRepository<GetAllStudentsInSemesterModel>(_context);
            GetAllByFacultyIdModels = new GeneralRepository<GetAllByFacultyIdModel>(_context);
            GetStudentsSemesterResultModels = new GeneralRepository<GetStudentsSemesterResultModel>(_context);
            GetStudentScheduleByUserIdModels = new GeneralRepository<GetStudentScheduleByUserIdModel>(_context);
            GetStudentInSemesterResultModels = new GeneralRepository<GetStudentInSemesterResultModel>(_context);
            GetCurrentStaffByCourseIdModels = new GeneralRepository<GetCurrentStaffByCourseIdModel>(_context);
            GetAllStudentInCourseResultModels = new GeneralRepository<GetAllStudentInCourseResultModel>(_context);
            GetStudentBySectionIdModels = new GeneralRepository<GetStudentBySectionIdModel>(_context);
            GetStudentInfoByStudentIdModels = new GeneralRepository<GetStudentInfoByStudentIdModel>(_context);
            GetStaffInfoByStaffIdModels = new GeneralRepository<GetStaffInfoByStaffIdModel>(_context);
            GetStudentCourseInfoModels = new GeneralRepository<GetStudentCourseInfoModel>(_context);
            GetGraduateStudentsByAcademyYearIdModels = new GeneralRepository<GetGraduateStudentsByAcademyYearIdModel>(_context);
            GetSchedulesForStaffByUserIdModels = new GeneralRepository<GetSchedulesForStaffByUserIdModel>(_context);
            GetScheduleDetailsModels = new GeneralRepository<GetScheduleDetailsModel>(_context);
            Cities = new GeneralRepository<City>(_context);
            Countries = new GeneralRepository<Country>(_context);
            Governorates = new GeneralRepository<Governorate>(_context);
            ActivityLogs = new GeneralRepository<ActivityLog>(_context);
            Results = new GeneralRepository<Result>(_context);
            SchedulesRepo = new SchedulesRepository(_context);
        }


        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
