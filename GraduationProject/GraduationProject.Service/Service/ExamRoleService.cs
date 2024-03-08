using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.ExamRolesDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class ExamRoleService : IExamRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExamRoleService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddExamRoleAsync(ExamRolesDto addExamRoleDto)
        {
            ExamRole newExamRole = new ExamRole
            {
                Name = addExamRoleDto.Name,
                Code = addExamRoleDto.Code,
                Order = addExamRoleDto.Order,
                FacultyId = addExamRoleDto.FacultyId
            };
            await _unitOfWork.ExamRoles.AddAsync(newExamRole);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IQueryable<ExamRolesDto>> GetExamRoleAsync()
        {
            var examRolesEntities = await _unitOfWork.ExamRoles.GetAll();

            var examRolesDtos = examRolesEntities.Select(entity => new ExamRolesDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Order = entity.Order,
                FacultyId = entity.FacultyId
            });

            return examRolesDtos.AsQueryable();
        }

        public async Task<ExamRolesDto> GetExamRoleByIdAsync(int ExamRoleId)
        {
            var examRolesEntity = await _unitOfWork.ExamRoles.GetByIdAsync(ExamRoleId);
            ExamRolesDto examRolesDto = new ExamRolesDto
            {
                Id = examRolesEntity.Id,
                Name = examRolesEntity.Name,
                Code = examRolesEntity.Code,
                Order = examRolesEntity.Order,
                FacultyId = examRolesEntity.FacultyId
            };
            return (examRolesDto);
        }

        public async Task UpdateExamRoleAsync(ExamRolesDto updateExamRoleDto)
        {
            ExamRole existingExamRole = await _unitOfWork.ExamRoles.GetByIdAsync(updateExamRoleDto.Id);
            if (existingExamRole == null)
            {
                throw new Exception("ExamRole not found");
            }
            existingExamRole.Name = updateExamRoleDto.Name;
            existingExamRole.Code = updateExamRoleDto.Code;
            existingExamRole.Order = updateExamRoleDto.Order;
            existingExamRole.FacultyId = updateExamRoleDto.FacultyId;

            await _unitOfWork.ExamRoles.Update(existingExamRole);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteExamRoleAsync(int ExamRoleId)
        {
            var existingExamRole = await _unitOfWork.ExamRoles.GetByIdAsync(ExamRoleId);
            await _unitOfWork.ExamRoles.Delete(existingExamRole);
            await _unitOfWork.SaveAsync();
        }
    }
}
