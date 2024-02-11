using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.SemesterDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SemesterService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddSemesterAsync(SemesterDto addSemesterDto)
        {
            Semester newSemester = new Semester
            {
                Name = addSemesterDto.Name,
                Code = addSemesterDto.Code,
                Order = addSemesterDto.Order,
                FacultyId = addSemesterDto.FacultyId
            };
            await _unitOfWork.Semesters.AddAsync(newSemester);
            _unitOfWork.Save();
        }


        public async Task<IQueryable<SemesterDto>> GetSemesterAsync()
        {
            var semesterEntities = await _unitOfWork.Semesters.GetAll();

            var semesterDtos = semesterEntities.Select(entity => new SemesterDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Order = entity.Order,
                FacultyId = entity.FacultyId
            });

            return semesterDtos.AsQueryable();
        }

        public async Task<SemesterDto> GetSemesterByIdAsync(int SemesterId)
        {
            var semesterEntity = await _unitOfWork.Semesters.GetByIdAsync(SemesterId);
            SemesterDto semesterDto = new SemesterDto
            {
                Id = semesterEntity.Id,
                Name = semesterEntity.Name,
                Code = semesterEntity.Code,
                Order = semesterEntity.Order,
                FacultyId = semesterEntity.FacultyId
            };
            return (semesterDto);
        }

        public async Task UpdateSemesterAsync(SemesterDto updateSemesterDto)
        {
            Semester existingSemester = await _unitOfWork.Semesters.GetByIdAsync(updateSemesterDto.Id);
            if (existingSemester == null)
            {
                throw new Exception("Semester not found");
            }
            existingSemester.Name = updateSemesterDto.Name;
            existingSemester.Code = updateSemesterDto.Code;
            existingSemester.Order = updateSemesterDto.Order;
            existingSemester.FacultyId = updateSemesterDto.FacultyId;

            await _unitOfWork.Semesters.Update(existingSemester);
            _unitOfWork.Save();
        }
        public async Task DeleteSemesterAsync(int SemesterId)
        {
            var existingSemester = await _unitOfWork.Semesters.GetByIdAsync(SemesterId);
            await _unitOfWork.Semesters.Delete(existingSemester);
            _unitOfWork.Save();
        }
    }
}
