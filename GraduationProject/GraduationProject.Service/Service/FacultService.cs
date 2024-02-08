using GraduationProject.Data.Entity;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.FacultyDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class FacultService : IFacultService
    {
        private readonly UnitOfWork _unitOfWork;
        public FacultService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task AddFacultAsync(FacultyDto facultyDto)
        {
            Faculty newFaculty = new Faculty
            {
                Name = facultyDto.Name,
                Description = facultyDto.Description,
                UserId = "3ed1410b-286c-4064-9193-35b792b8aebf"
            };
            await _unitOfWork.Facultys.AddAsync(newFaculty);
            _unitOfWork.Save();
        }


    }
}
