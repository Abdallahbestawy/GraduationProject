using GraduationProject.Repository.Repository;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class Department : IDepartment
    {
        private readonly UnitOfWork _unitOfWork;
        public Department(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task AddDepartmentAsync(Data.Entity.Department entity)
        {
            await _unitOfWork.Departments.AddAsync(entity);
            _unitOfWork.Save();
        }
    }
}
