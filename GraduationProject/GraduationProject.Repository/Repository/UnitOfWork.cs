using GraduationProject.Data.Entity;
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
        public IGeneralRepository<Phase> Phases { get; private set; }
        public IGeneralRepository<ExamRole> ExamRoles { get; private set; }
        public IGeneralRepository<ScientificDegree> ScientificDegrees { get; private set; }
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
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
