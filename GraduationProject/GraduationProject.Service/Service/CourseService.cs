using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.CourseDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

namespace GraduationProject.Service.Service
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public CourseService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }

        public async Task<Response<int>> AddCourseAsync(CourseDto addCourseDto)
        {
            Course newCourse = new Course
            {
                Name = addCourseDto.Name,
                Code = addCourseDto.Code,
                Description = addCourseDto.Description,
                Type = addCourseDto.Type,
                Category = addCourseDto.Category,
                MaxDegree = addCourseDto.MaxDegree,
                MinDegree = addCourseDto.MinDegree,
                NumberOfCreditHours = addCourseDto.NumberOfCreditHours,
                NumberOfPoints = addCourseDto.NumberOfPoints,
                Prerequisite = addCourseDto.Prerequisite,
                ScientificDegreeId = addCourseDto.ScientificDegreeId,
                DepartmentId = addCourseDto.DepartmentId
            };

            try
            {
                await _unitOfWork.Courses.AddAsync(newCourse);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "AddCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding course",
                     "An unexpected error occurred while adding course. Please try again later.");
            }
            try
            {
                if (addCourseDto.Prerequisite && addCourseDto.CoursePrerequisites != null)
                {
                    int courseId = newCourse.Id;
                    List<CoursePrerequisite> coursePrerequisites = addCourseDto.CoursePrerequisites.Select(p =>
                    new CoursePrerequisite
                    {
                        CourseId = courseId,
                        PrerequisiteId = p.CoursePrerequisiteId
                    }).ToList();
                    await _unitOfWork.CoursePrerequisites.AddRangeAsync(coursePrerequisites);
                    await _unitOfWork.SaveAsync();
                }
                return Response<int>.Created("Course added successfully");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "AddCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                await _unitOfWork.Courses.Delete(newCourse);
                return Response<int>.ServerError("Error occured while adding course",
                     "An unexpected error occurred while adding course. Please try again later.");
            }
        }

        public async Task<Response<GetCourseByIdDto>> GetCourseByIdAsync(int CourseId)
        {
            try
            {
                var courseEntity = await _unitOfWork.Courses.GetByIdAsync(CourseId);

                if (courseEntity == null)
                    return Response<GetCourseByIdDto>.BadRequest("This course doesn't exist");

                GetCourseByIdDto courseDto = new GetCourseByIdDto
                {
                    Id = courseEntity.Id,
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
                var coursePrerequisites = await _unitOfWork.CoursePrerequisites.GetEntityByPropertyAsync(c => c.CourseId == courseEntity.Id);
                if (coursePrerequisites.Any())
                {
                    var preCourse = await _unitOfWork.CoursePrerequisites
                   .FindWithIncludeIEnumerableAsync(
                       c => c.Course,
                       c => c.Prerequisite
                   );
                    courseDto.CoursePrerequisites = preCourse.Where(c => c.CourseId == courseEntity.Id).Select(pc => new GetCoursePrerequisiteDto
                    {
                        CoursePrerequisiteId = pc.Id,
                        CoursePrerequisiteName = pc.Prerequisite.Name
                    }).ToList();
                }

                return Response<GetCourseByIdDto>.Success(courseDto, "Course retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetCourseByIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetCourseByIdDto>.ServerError("Error occured while retrieving course",
                    "An unexpected error occurred while retrieving course. Please try again later.");
            }
        }

        public async Task<Response<IQueryable<CourseDto>>> GetCoursesAsync()
        {
            try
            {
                var bandEntities = await _unitOfWork.Courses.GetAll();

                if (!bandEntities.Any())
                    return Response<IQueryable<CourseDto>>.NoContent("No courses are exist");

                var courseDto = bandEntities.Select(entity => new CourseDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Code = entity.Code,
                    Description = entity.Description,
                    Type = entity.Type,
                    Category = entity.Category,
                    MaxDegree = entity.MaxDegree,
                    MinDegree = entity.MinDegree,
                    NumberOfCreditHours = entity.NumberOfCreditHours,
                    NumberOfPoints = entity.NumberOfPoints,
                    Prerequisite = entity.Prerequisite,
                    ScientificDegreeId = entity.ScientificDegreeId,
                    DepartmentId = entity.DepartmentId
                });

                return Response<IQueryable<CourseDto>>.Success(courseDto.AsQueryable(), "Courses retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetCoursesAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<IQueryable<CourseDto>>.ServerError("Error occured while retrieving courses",
                    "An unexpected error occurred while retrieving courses. Please try again later.");
            }
        }

        public async Task<Response<int>> UpdateCourseAsync(CourseDto updateCourseDto)
        {
            try
            {
                Course existingCourse = await _unitOfWork.Courses.GetByIdAsync(updateCourseDto.Id);

                if (existingCourse == null)
                    return Response<int>.BadRequest("This course doesn't exist");

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
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Updated("course updated successfully");

                return Response<int>.ServerError("Error occured while updating course",
                        "An unexpected error occurred while updating course. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "UpdateCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating courses",
                    "An unexpected error occurred while updating courses. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteCourseAsync(int CourseId)
        {
            try
            {
                var existingCourse = await _unitOfWork.Courses.GetByIdAsync(CourseId);

                if (existingCourse == null)
                    return Response<int>.BadRequest("This course doesn't exist");

                await _unitOfWork.Courses.Delete(existingCourse);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("course deleted successfully");

                return Response<int>.ServerError("Error occured while deleting course",
                        "An unexpected error occurred while deleting course. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "DeleteCourseAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting course",
                        "An unexpected error occurred while deleting course. Please try again later.");
            }
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

        public async Task<Response<int>> AddCourseAssessMethodAsync(CourseAssessMethodDto addCourseAssessMethodDto)
        {
            try
            {
                List<CourseAssessMethod> newCourseAssessMethods = addCourseAssessMethodDto.CourseAssessMethods.Select(ca =>
                new CourseAssessMethod
                {
                    CourseId = ca.CourseId,
                    AssessMethodId = ca.AssessMethodsId
                }).ToList();
                await _unitOfWork.CourseAssessMethods.AddRangeAsync(newCourseAssessMethods);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Created("course assigned to assess methods successfully");

                return Response<int>.ServerError("Error occured while assigning to assess methods to course",
                        "An unexpected error occurred while assigning to assess methods to course. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "AddCourseAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting course",
                        "An unexpected error occurred while deleting course. Please try again later.");
            }
        }

        public async Task<CourseAssessMethodDto> GetAssessMethodsByCoursesIdAsync(int courseId)
        {
            var courseAssessMethodsTask = _unitOfWork.CourseAssessMethods
                 .FindAllByForeignKeyAsync(c => c.CourseId, courseId);

            var courseAssessMethods = await courseAssessMethodsTask;

            var assessMethodDtos = courseAssessMethods
                .Select(assessMethod => new CourseAssessMethodDtos
                {
                    Id = assessMethod.Id,
                    CourseId = assessMethod.CourseId,
                    AssessMethodsId = assessMethod.AssessMethodId
                }).ToList();

            return new CourseAssessMethodDto
            {
                CourseAssessMethods = assessMethodDtos
            };

        }

        public async Task<Response<CourseStudentsAssessMethodDto>> GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus(int courseId, bool isControlStatus)
        {
            try
            {
                SqlParameter pCourseId = new SqlParameter("@CourseId", courseId);
                SqlParameter pIsControlStatus = new SqlParameter("@IsControlStatus", isControlStatus);

                var courseStudentAssessMethods = await _unitOfWork.GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatusModels
                    .CallStoredProcedureAsync("EXECUTE SpGetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus", pCourseId, pIsControlStatus);

                if (courseStudentAssessMethods == null)
                    return Response<CourseStudentsAssessMethodDto>.NoContent("No Student's assess methods with degrees are exist");

                var courseStudentAssessMethodDto = new CourseStudentsAssessMethodDto
                {
                    CourseName = courseStudentAssessMethods.FirstOrDefault()?.CourseName,
                    CourseCode = courseStudentAssessMethods.FirstOrDefault()?.CourseCode,
                    StudentDtos = courseStudentAssessMethods
                        .AsEnumerable()
                        .GroupBy(s => new { s.StudentName })
                        .Select(group => new StudentDto
                        {
                            StudentName = group.Key.StudentName,
                            StudentCode = group.FirstOrDefault().StudentCode,
                            AssesstMethodDtos = group.Select(s => new AssesstMethodDto
                            {
                                StudentSemesterAssessMethodId = s.StudentSemesterAssessMethodsId,
                                AssessName = s.AssessmentMethodName,
                                AssessDegree = s.Degree
                            }).ToList()
                        }).ToList()
                };

                return Response<CourseStudentsAssessMethodDto>
                    .Success(courseStudentAssessMethodDto, "Student's assess methods with degrees retrieved successfully")
                    .WithCount(courseStudentAssessMethodDto.StudentDtos.Count());
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetStudentSemesterAssessMethodsBySpecificCourseAndControlStatus",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<CourseStudentsAssessMethodDto>.ServerError("Error occured while retrieving Student's assess methods with degrees",
                    "An unexpected error occurred while retrieving Student's assess methods with degrees. Please try again later.");
            }
        }

        public async Task<Response<List<GetCourseDto>>> GetCoursePrerequisiteAsync(int courseId)
        {
            try
            {
                if (await _unitOfWork.Courses.GetByIdAsync(courseId) == null)
                    return Response<List<GetCourseDto>>.BadRequest("This course doesn't exist");

                var preCourse = await _unitOfWork.CoursePrerequisites
                    .FindWithIncludeIEnumerableAsync(
                        c => c.Course,
                        c => c.Prerequisite
                    );
                if (preCourse == null || !preCourse.Any())
                    return Response<List<GetCourseDto>>.NoContent("No Prerequisites are exist");

                preCourse = preCourse.Where(c => c.CourseId == courseId);
                if (preCourse == null || !preCourse.Any())
                    return Response<List<GetCourseDto>>.NoContent("No Prerequisites are exist for this course");

                List<GetCourseDto> getCoursePrerequisiteDtos = preCourse
                    .Select(cs => new GetCourseDto
                    {
                        CourseId = cs.Prerequisite.Id,
                        CourseName = cs.Prerequisite.Name,
                        CourseCode = cs.Prerequisite.Code
                    }).ToList();

                return Response<List<GetCourseDto>>.Success(getCoursePrerequisiteDtos, "Prerequisites are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetCoursePrerequisiteAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetCourseDto>>.ServerError("Error occured while retrieving course's Prerequisites",
                    "An unexpected error occurred while retrieving course's Prerequisites. Please try again later.");
            }
        }

        public async Task<Response<List<GetCourseDto>>> GetCourseBySemesterIdAsync(int semesterId)
        {
            try
            {
                if (await _unitOfWork.ScientificDegrees.GetByIdAsync(semesterId) == null)
                    return Response<List<GetCourseDto>>.BadRequest("This semester doesn't exist");

                var courses = await _unitOfWork.Courses.GetEntityByPropertyAsync(c => c.ScientificDegreeId == semesterId);

                if (courses == null || !courses.Any())
                    return Response<List<GetCourseDto>>.NoContent("No coureses are exist");

                var getCourseDtos = courses.Select(course => new GetCourseDto
                {
                    CourseId = course.Id,
                    CourseName = course.Name,
                    CourseCode = course.Code
                }).ToList();

                return Response<List<GetCourseDto>>.Success(getCourseDtos, "Courses are retrieved successfully").WithCount();
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetCourseBySemesterIdAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetCourseDto>>.ServerError("Error occured while retrieving semester's courses",
                    "An unexpected error occurred while retrieving semester's courses. Please try again later.");
            }
        }

        public async Task<Response<bool>> UpdateCourseStudentsAssessMethodAsync(List<UpdateCourseStudentsAssessMethodDto> updateCourseStudentsAssessMethodDto)
        {
            if (updateCourseStudentsAssessMethodDto == null || !updateCourseStudentsAssessMethodDto.Any())
                return Response<bool>.BadRequest("Please enter valid degrees");

            try
            {
                var entitiesToUpdate = new List<StudentSemesterAssessMethod>();

                foreach (var asDto in updateCourseStudentsAssessMethodDto)
                {
                    var old = await _unitOfWork.StudentSemesterAssessMethods.GetByIdAsync(asDto.StudentSemesterAssessMethodId);
                    if (old != null)
                    {
                        old.Degree = asDto.Degree;
                        entitiesToUpdate.Add(old);
                    }
                }

                if (!entitiesToUpdate.Any())
                    return Response<bool>.BadRequest("Student Semester Assess Methods doesn't exist");

                bool result = await _unitOfWork.StudentSemesterAssessMethods.UpdateRangeAsync(entitiesToUpdate);

                if (!result)
                    return Response<bool>.ServerError("Error occured while updating Student Semester Assess Methods",
                    "An unexpected error occurred while updating Student Semester Assess Methods. Please try again later.");

                int respons = await _unitOfWork.SaveAsync();
                if (respons < 0)
                {
                    return Response<bool>.ServerError("Error occured while updating Student Semester Assess Methods",
                  "An unexpected error occurred while updating Student Semester Assess Methods. Please try again later.");
                }

                return Response<bool>.Updated("Student Semester Assess Methods updated successfully");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "UpdateCourseStudentsAssessMethodAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while updating Student Semester Assess Methods",
                    "An unexpected error occurred while updating Student Semester Assess Methods. Please try again later.");
            }
        }

        public async Task<Response<GetStudentCourseInfoDto>> GetStudentCourseInfoAsync(int courseId)
        {
            try
            {
                SqlParameter pCourseId = new SqlParameter("@CourseId", courseId);
                var studentSemesterCourse = await _unitOfWork.GetStudentCourseInfoModels.CallStoredProcedureAsync(
                        "EXECUTE SpGetStudentCourseInfo", pCourseId);

                if (studentSemesterCourse == null)
                    return Response<GetStudentCourseInfoDto>.NoContent();

                var getStudentSemesterCoursesDtos = new GetStudentCourseInfoDto
                {
                    CourseName = studentSemesterCourse.FirstOrDefault().CourseName,
                    CourseCode = studentSemesterCourse.FirstOrDefault().CourseCode,
                    studentCourseInfoDetials = studentSemesterCourse.Select(sc => new StudentCourseInfoDetialsDto
                    {
                        StudentSemesterCourseId = sc.StudentSemesterCourseId,
                        StudentCode = sc.StudentCode,
                        StudentName = sc.StudentName,
                        Notes = sc.Notes
                    }).ToList(),
                };

                return Response<GetStudentCourseInfoDto>.Success(getStudentSemesterCoursesDtos, "Students course info are retrieved successfully")
                    .WithCount(getStudentSemesterCoursesDtos.studentCourseInfoDetials.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "GetStudentCourseInfoAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetStudentCourseInfoDto>.ServerError("Error occured while retrieving students course info",
                    "An unexpected error occurred while retrieving students course info. Please try again later.");
            }
        }

        public async Task<Response<bool>> UpdateStudentCourseInfoAsync(List<UpdateStudentCourseInfoDto> updateStudentCourseInfoDtos)
        {
            try
            {
                if (updateStudentCourseInfoDtos == null || !updateStudentCourseInfoDtos.Any())
                    return Response<bool>.BadRequest("Invalid input");

                var entitiesToUpdate = new List<StudentSemesterCourse>();

                foreach (var asDto in updateStudentCourseInfoDtos)
                {
                    var old = await _unitOfWork.StudentSemesterCourses.GetByIdAsync(asDto.studentSemesterCourseId);
                    if (old != null)
                    {
                        old.Notes = asDto.Notes;
                        entitiesToUpdate.Add(old);
                    }
                }

                if (!entitiesToUpdate.Any())
                    return Response<bool>.NoContent("There is no updates to do");

                bool result = await _unitOfWork.StudentSemesterCourses.UpdateRangeAsync(entitiesToUpdate);

                if (!result)
                    return Response<bool>.ServerError("Error occured while updating student course info",
                        "An unexpected error occurred while updating student course info. Please try again later.");

                int respons = await _unitOfWork.SaveAsync();
                if (respons < 0)
                    return Response<bool>.ServerError("Error occured while updating student course info",
                        "An unexpected error occurred while updating student course info. Please try again later.");

                return Response<bool>.Updated("The student course info is updated successfully");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "UpdateStudentCourseInfoAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while updating student course info",
                    "An unexpected error occurred while updating student course info. Please try again later.");
            }
        }

        public async Task<Response<int>> DeleteCoursePrerequisitesAsync(int coursePrerequisitesId)
        {
            try
            {
                var existingCourse = await _unitOfWork.CoursePrerequisites.GetByIdAsync(coursePrerequisitesId);

                if (existingCourse == null)
                    return Response<int>.BadRequest("This Course Prerequisites doesn't exist");

                await _unitOfWork.CoursePrerequisites.Delete(existingCourse);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                    return Response<int>.Deleted("Course Prerequisites deleted successfully");

                return Response<int>.ServerError("Error occured while deleting Course Prerequisites",
                        "An unexpected error occurred while deleting Course Prerequisites. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = "CourseService",
                    MethodName = "DeleteCoursePrerequisitesAsync",
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting Course Prerequisites",
                        "An unexpected error occurred while deleting Course Prerequisites. Please try again later.");
            }
        }
    }
}
