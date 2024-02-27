using GraduationProject.Data.Entity;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.IService;

namespace GraduationProject.Service.Service
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CourseService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        }
        public async Task AddCourseAsync(CourseDto addCourseDto)
        {
            Course newCourse = new Course
            {
                Name = addCourseDto.Name,
                Code = addCourseDto.Code,
                Description = addCourseDto.Description,
                Type = addCourseDto.Type,
                Category = addCourseDto.Category,
                MaxDegree = addCourseDto.MaxDegree,
                NumberOfCreditHours = addCourseDto.NumberOfCreditHours,
                NumberOfPoints = addCourseDto.NumberOfPoints,
                Prerequisite = addCourseDto.Prerequisite,
                ScientificDegreeId = addCourseDto.ScientificDegreeId,
                DepartmentId = addCourseDto.DepartmentId
            };
            await _unitOfWork.Courses.AddAsync(newCourse);
            _unitOfWork.Save();
            if (addCourseDto.Prerequisite && addCourseDto.CoursePrerequisites != null)
            {
                int courseId = newCourse.Id;
                List<CoursePrerequisite> coursePrerequisites = addCourseDto.CoursePrerequisites.Select(p =>
                new CoursePrerequisite
                {
                    CourseId = courseId,
                    PrerequisiteId = p.CoursePrerequisiteId
                }).ToList();
                _unitOfWork.CoursePrerequisites.AddRangeAsync(coursePrerequisites);
                _unitOfWork.Save();
            }

        }

        public async Task<CourseDto> GetCourseByIdAsync(int CourseId)
        {
            var courseEntity = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            CourseDto courseDto = new CourseDto
            {
                Name = courseEntity.Name,
                Code = courseEntity.Code,
                Description = courseEntity.Description,
                Type = courseEntity.Type,
                Category = courseEntity.Category,
                MaxDegree = courseEntity.MaxDegree,
                NumberOfCreditHours = courseEntity.NumberOfCreditHours,
                NumberOfPoints = courseEntity.NumberOfPoints,
                Prerequisite = courseEntity.Prerequisite,
                ScientificDegreeId = courseEntity.ScientificDegreeId,
                DepartmentId = courseEntity.DepartmentId
            };
            return (courseDto);
        }

        public async Task<IQueryable<CourseDto>> GetCoursesAsync()
        {
            var bandEntities = await _unitOfWork.Courses.GetAll();

            var courseDto = bandEntities.Select(entity => new CourseDto
            {
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                Type = entity.Type,
                Category = entity.Category,
                MaxDegree = entity.MaxDegree,
                NumberOfCreditHours = entity.NumberOfCreditHours,
                NumberOfPoints = entity.NumberOfPoints,
                Prerequisite = entity.Prerequisite,
                ScientificDegreeId = entity.ScientificDegreeId,
                DepartmentId = entity.DepartmentId
            });

            return courseDto.AsQueryable();
        }

        public async Task UpdateCourseAsync(CourseDto updateCourseDto)
        {
            Course existingCourse = await _unitOfWork.Courses.GetByIdAsync(updateCourseDto.Id);
            if (existingCourse == null)
            {
                throw new Exception("Band not found");
            }
            existingCourse.Name = updateCourseDto.Name;
            existingCourse.Code = updateCourseDto.Code;
            existingCourse.Description = updateCourseDto.Description;
            existingCourse.Type = updateCourseDto.Type;
            existingCourse.Category = updateCourseDto.Category;
            existingCourse.MaxDegree = updateCourseDto.MaxDegree;
            existingCourse.NumberOfCreditHours = updateCourseDto.NumberOfCreditHours;
            existingCourse.NumberOfPoints = updateCourseDto.NumberOfPoints;
            existingCourse.Prerequisite = updateCourseDto.Prerequisite;
            existingCourse.ScientificDegreeId = updateCourseDto.ScientificDegreeId;
            existingCourse.DepartmentId = updateCourseDto.DepartmentId;

            await _unitOfWork.Courses.Update(existingCourse);
            _unitOfWork.Save();
        }
        public async Task DeleteCourseAsync(int CourseId)
        {
            var existingCourse = await _unitOfWork.Courses.GetByIdAsync(CourseId);
            await _unitOfWork.Courses.Delete(existingCourse);
            _unitOfWork.Save();
        }

        public async Task<IQueryable<CourseDto>> GetCoursesByScientificDegreeIdAsync(int scientificDegreeId)
        {
            var courses = await _unitOfWork.Courses.FindAllByForeignKeyAsync(c => c.ScientificDegreeId, scientificDegreeId);
            var courseDto = courses.Select(entity => new CourseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                Type = entity.Type,
                Category = entity.Category,
                MaxDegree = entity.MaxDegree,
                NumberOfCreditHours = entity.NumberOfCreditHours,
                NumberOfPoints = entity.NumberOfPoints,
                Prerequisite = entity.Prerequisite,
                ScientificDegreeId = entity.ScientificDegreeId,
                DepartmentId = entity.DepartmentId
            });

            return courseDto.AsQueryable();
        }

    }
}
