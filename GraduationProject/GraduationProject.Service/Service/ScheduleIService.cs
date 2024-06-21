using GraduationProject.Data.Entity;
using GraduationProject.Mails.IService;
using GraduationProject.Mails.Models;
using GraduationProject.Repository.IRepository;
using GraduationProject.Repository.Repository;
using GraduationProject.ResponseHandler.Model;
using GraduationProject.Service.DataTransferObject.ScheduleDto;
using GraduationProject.Service.IService;
using Microsoft.Data.SqlClient;

namespace GraduationProject.Service.Service
{
    public class ScheduleIService : IScheduleIService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;

        public ScheduleIService(UnitOfWork unitOfWork, IMailService mailService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mailService = mailService;
        }
        public async Task<Response<int>> AddScheduleAsync(ScheduleDto addScheduleDto)
        {
            try
            {
                List<Schedule> schedules = new List<Schedule>();
                var place = await _unitOfWork.SchedulePlaces.GetEntityByPropertyAsync(d => d.FacultyId == addScheduleDto.ScheduleDetails.FirstOrDefault().FacultyId);
                if (!place.Any())
                {
                    return Response<int>.BadRequest("There are no places");
                }
                var academyYear = await _unitOfWork.AcademyYears.GetEntityByPropertyAsync(f => f.FacultyId == addScheduleDto.ScheduleDetails.FirstOrDefault().FacultyId && f.IsCurrent);
                if (!academyYear.Any())
                {
                    return Response<int>.BadRequest("There is no current Academic year");
                }
                var academyYearId = academyYear.FirstOrDefault().Id;
                var oldschedules = await _unitOfWork.Schedules.GetEntityByPropertyAsync(f => f.FacultyId == addScheduleDto.ScheduleDetails.FirstOrDefault().FacultyId && f.AcademyYearId == academyYearId);
                foreach (var schedule in addScheduleDto.ScheduleDetails)
                {
                    bool rejected = await IsScheduleRejected(oldschedules.ToList(), schedule, schedules);
                    if (rejected)
                    {
                        return Response<int>.BadRequest("The schedule is unavailable at the selected time. Please choose a different time slot.");
                    }
                    bool rejectedPlace = await IsSchedulePlaceRejected(place.FirstOrDefault(p => p.Id == schedule.SchedulePlaceId).PlaceCapacity, schedule.Capacity);
                    if (rejectedPlace)
                    {
                        return Response<int>.BadRequest("The entered capacity exceeds the maximum limit for this place. Please enter a value within the allowed capacity.");
                    }
                    Schedule newschedule = new Schedule
                    {
                        ScientificDegreeId = addScheduleDto.SemesterDegreeId,
                        ScheduleDay = schedule.ScheduleDay,
                        SchedulePlaceId = schedule.SchedulePlaceId,
                        CourseId = schedule.CourseId,
                        StaffId = schedule.StaffId,
                        AcademyYearId = academyYearId,
                        ScheduleType = schedule.ScheduleType,
                        TimeStart = new TimeSpan(schedule.StartHour, schedule.StartMinute, 0),
                        EndStart = new TimeSpan(schedule.EndHour, schedule.EndMinute, 0),
                        Capacity = schedule.Capacity,
                        FacultyId = schedule.FacultyId,
                    };
                    schedules.Add(newschedule);
                }
                await _unitOfWork.Schedules.AddRangeAsync(schedules);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Response<int>.Created("The schedule added successfully");
                }
                return Response<int>.ServerError("Error occured while adding the schedule",
                         "An unexpected error occurred while adding the schedule. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(AddScheduleAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while adding the schedule",
                         "An unexpected error occurred while adding the schedule. Please try again later.");
            }
        }
        public async Task<Response<ScheduleDto>> GetScheduleBySemesterIdAsync(int semesterId, int factlyId)
        {
            try
            {
                var schedul = await _unitOfWork.Schedules.GetEntityByPropertyWithIncludeAsync(
                     d => d.ScientificDegreeId == semesterId && d.AcademyYear.IsCurrent && d.FacultyId == factlyId,
                     a => a.AcademyYear
                );
                if (!schedul.Any())
                {
                    return Response<ScheduleDto>.NoContent();
                }
                ScheduleDto getScheduleDto = new ScheduleDto
                {
                    SemesterDegreeId = schedul.FirstOrDefault().ScientificDegreeId,
                    AcademyYearId = schedul.FirstOrDefault().AcademyYearId,
                    ScheduleDetails = schedul.Select(s => new ScheduleDetailsDto
                    {
                        Id = s.Id,
                        ScheduleType = s.ScheduleType,
                        ScheduleDay = s.ScheduleDay,
                        StartHour = s.TimeStart.Hours,
                        StartMinute = s.TimeStart.Minutes,
                        EndHour = s.EndStart.Hours,
                        EndMinute = s.EndStart.Minutes,
                        Capacity = s.Capacity,
                        FacultyId = s.FacultyId,
                        StaffId = s.StaffId,
                        SchedulePlaceId = s.SchedulePlaceId,
                        CourseId = s.CourseId
                    }).ToList()
                };

                return Response<ScheduleDto>.Success(getScheduleDto, "Schedule retrieved successfully").WithCount(getScheduleDto.ScheduleDetails.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(GetScheduleBySemesterIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<ScheduleDto>.ServerError("Error occured while retrieving the schedule",
                         "An unexpected error occurred while retrieving the schedule. Please try again later.");
            }
        }

        public async Task<Response<GetSchedulesForStaffByUserIdDto>> GetSchedulesForStaffByUserIdAsync(string userId)
        {
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);

                var getSchedulesForStaff = await _unitOfWork.GetSchedulesForStaffByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetSchedulesForStaffByUserId", pUserId);
                if (!getSchedulesForStaff.Any())
                {
                    return Response<GetSchedulesForStaffByUserIdDto>.NoContent();
                }
                GetSchedulesForStaffByUserIdDto getSchedulesForStaffByUserIdDto = new GetSchedulesForStaffByUserIdDto
                {
                    AcademyYearName = getSchedulesForStaff.FirstOrDefault().AcademyYear,
                    FacultysName = getSchedulesForStaff.FirstOrDefault().FacultysName,
                    NameEnglish = getSchedulesForStaff.FirstOrDefault().NameEnglish,
                    getSchedulesForStaffByUserIdDetails = getSchedulesForStaff.Select(s => new GetSchedulesForStaffByUserIdDetailsDto
                    {
                        SchedulesId = s.SchedulesId,
                        CoursesCode = s.CoursesCode,
                        CoursesName = s.CoursesName,
                        ScheduleDay = s.ScheduleDay,
                        SchedulePlacesName = s.SchedulePlacesName,
                        ScientificDegreesName = $"{s.BandName} - {s.ScientificDegreesName}",
                        ScheduleType = s.ScheduleType,
                        Timing = s.Timing,
                    }).ToList()
                };
                return Response<GetSchedulesForStaffByUserIdDto>.Success(getSchedulesForStaffByUserIdDto, "The schedule retrieved successfully")
                    .WithCount(getSchedulesForStaffByUserIdDto.getSchedulesForStaffByUserIdDetails.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(GetSchedulesForStaffByUserIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetSchedulesForStaffByUserIdDto>.ServerError("Error occured while retrieving the schedule",
                         "An unexpected error occurred while retrieving the schedule. Please try again later.");
            }
        }
        public async Task<Response<int>> UpdateScheduleAsync(ScheduleDto updateScheduleDto)
        {
            try
            {
                List<Schedule> schedules = new List<Schedule>();
                var oldschedules = await _unitOfWork.Schedules.GetEntityByPropertyWithIncludeAsync(
                    f => f.FacultyId == updateScheduleDto.ScheduleDetails.FirstOrDefault().FacultyId && f.AcademyYearId == updateScheduleDto.AcademyYearId,
                    p => p.SchedulePlace
                    );
                foreach (var schedule in updateScheduleDto.ScheduleDetails)
                {
                    Schedule existSchedule = await _unitOfWork.Schedules.GetByIdAsync(schedule.Id);
                    if (existSchedule == null)
                    {
                        return Response<int>.BadRequest("This Schedule doesn't exist");
                    }
                    bool rejected = await IsScheduleRejected(oldschedules.ToList(), schedule, schedules);
                    if (rejected)
                    {
                        return Response<int>.BadRequest("The schedule is unavailable at the selected time. Please choose a different time slot.");
                    }
                    bool rejectedPlace = await IsSchedulePlaceRejected(oldschedules.FirstOrDefault(p => p.Id == schedule.SchedulePlaceId).SchedulePlace.PlaceCapacity, schedule.Capacity);
                    if (rejectedPlace)
                    {
                        return Response<int>.BadRequest("The entered capacity exceeds the maximum limit for this place. Please enter a value within the allowed capacity.");
                    }
                    existSchedule.ScientificDegreeId = updateScheduleDto.SemesterDegreeId;
                    existSchedule.ScheduleDay = schedule.ScheduleDay;
                    existSchedule.SchedulePlaceId = schedule.SchedulePlaceId;
                    existSchedule.CourseId = schedule.CourseId;
                    existSchedule.StaffId = schedule.StaffId;
                    existSchedule.ScheduleType = schedule.ScheduleType;
                    existSchedule.TimeStart = new TimeSpan(schedule.StartHour, schedule.StartMinute, 0);
                    existSchedule.EndStart = new TimeSpan(schedule.EndHour, schedule.EndMinute, 0);
                    existSchedule.Capacity = schedule.Capacity;
                    existSchedule.FacultyId = schedule.FacultyId;
                    schedules.Add(existSchedule);
                }
                await _unitOfWork.Schedules.UpdateRangeAsync(schedules);
                int result = await _unitOfWork.SaveAsync();
                if (result > 0)
                {
                    return Response<int>.Updated("The schedule updated successfully");
                }
                return Response<int>.ServerError("Error occured while updating the schedule",
                             "An unexpected error occurred while updating the schedule. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(UpdateScheduleAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while updating the schedule",
                             "An unexpected error occurred while updating the schedule. Please try again later.");
            }
        }
        public async Task<Response<int>> DeleteScheduleAsync(int scheduleId)
        {
            try
            {
                var existingSchedule = await _unitOfWork.Schedules.GetByIdAsync(scheduleId);

                if (existingSchedule == null)
                {
                    return Response<int>.BadRequest("This schedule doesn't exist");
                }

                await _unitOfWork.Schedules.Delete(existingSchedule);
                var result = await _unitOfWork.SaveAsync();

                if (result > 0)
                {
                    return Response<int>.Deleted("The schedule deleted successfully");
                }
                return Response<int>.ServerError("Error occured while deleting the schedule",
                                 "An unexpected error occurred while deleting the schedule. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(DeleteScheduleAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<int>.ServerError("Error occured while deleting the schedule",
                                 "An unexpected error occurred while deleting the schedule. Please try again later.");
            }
        }
        private async Task<bool> IsSchedulePlaceRejected(int VaildPlace, int InputPlace)
        {
            if (InputPlace <= VaildPlace)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> IsScheduleRejected(List<Schedule> oldSchedules, ScheduleDetailsDto scheduleDetailsDto, List<Schedule> newSchedules)
        {
            if (!oldSchedules.Any() && !newSchedules.Any())
            {
                return false;
            }

            var TimeStart = new TimeSpan(scheduleDetailsDto.StartHour, scheduleDetailsDto.StartMinute, 0);
            var TimeEnd = new TimeSpan(scheduleDetailsDto.EndHour, scheduleDetailsDto.EndMinute, 0);
            if (oldSchedules.Any() && newSchedules.Any())
            {
                List<(TimeSpan start, TimeSpan end)> rangesold = new List<(TimeSpan, TimeSpan)>();
                foreach (var schedule in oldSchedules)
                {
                    rangesold.Add((schedule.TimeStart, schedule.EndStart));
                }
                List<(TimeSpan start, TimeSpan end)> rangesnew = new List<(TimeSpan, TimeSpan)>();
                foreach (var schedule in newSchedules)
                {
                    rangesnew.Add((schedule.TimeStart, schedule.EndStart));
                }
                if (oldSchedules.Any(s => s.StaffId == scheduleDetailsDto.StaffId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesold);
                }
                if (oldSchedules.Any(s => s.SchedulePlaceId == scheduleDetailsDto.SchedulePlaceId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesold);
                }
                if (newSchedules.Any(s => s.StaffId == scheduleDetailsDto.StaffId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesnew);
                }
                if (newSchedules.Any(s => s.SchedulePlaceId == scheduleDetailsDto.SchedulePlaceId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesnew);
                }

            }
            else if (!oldSchedules.Any())
            {
                List<(TimeSpan start, TimeSpan end)> rangesnew = new List<(TimeSpan, TimeSpan)>();
                foreach (var schedule in newSchedules)
                {
                    rangesnew.Add((schedule.TimeStart, schedule.EndStart));
                }
                if (newSchedules.Any(s => s.StaffId == scheduleDetailsDto.StaffId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesnew);
                }
                if (newSchedules.Any(s => s.SchedulePlaceId == scheduleDetailsDto.SchedulePlaceId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, rangesnew);
                }

            }
            else
            {
                List<(TimeSpan start, TimeSpan end)> ranges = new List<(TimeSpan, TimeSpan)>();
                foreach (var schedule in oldSchedules)
                {
                    ranges.Add((schedule.TimeStart, schedule.EndStart));
                }

                if (oldSchedules.Any(s => s.StaffId == scheduleDetailsDto.StaffId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, ranges);
                }
                if (oldSchedules.Any(s => s.SchedulePlaceId == scheduleDetailsDto.SchedulePlaceId && s.ScheduleDay == scheduleDetailsDto.ScheduleDay))
                {
                    return IsRangeRejected(TimeStart, TimeEnd, ranges);
                }

            }
            return false;
        }
        private bool IsRangeRejected(TimeSpan inputStart, TimeSpan inputEnd, List<(TimeSpan start, TimeSpan end)> ranges)
        {
            foreach (var range in ranges)
            {
                if (IsNumberInRange(inputStart, range) || IsNumberInRange(inputEnd, range))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsNumberInRange(TimeSpan number, (TimeSpan start, TimeSpan end) range)
        {
            if (range.start > range.end)
            {
                return number > range.start || number < range.end;
            }
            else
            {
                return number > range.start && number < range.end;
            }
        }

        public async Task<Response<bool>> AssignStudentsToSchedule(int ScientificDegreeId)
        {
            try
            {
                var result = await _unitOfWork.SchedulesRepo.AssignStudentsToSchedule(ScientificDegreeId);
                if (result)
                {
                    if (await _unitOfWork.SaveAsync() > 0)
                    {
                        return Response<bool>.Created("Students assigned to schedule successfully");
                    }
                }
                return Response<bool>.ServerError("Error occured while assigning students to schedule",
                                     "An unexpected error occurred while assigning students to schedule. Please try again later.");
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(AssignStudentsToSchedule),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<bool>.ServerError("Error occured while assigning students to schedule",
                                     "An unexpected error occurred while assigning students to schedule. Please try again later.");
            }
        }

        public async Task<Response<List<GetStudentScheduleByUserIdDto>>> GetStudentScheduleByUserIdAsync(string userId)
        {
            try
            {
                SqlParameter pUserId = new SqlParameter("@UserId", userId);

                var getStudentSchedule = await _unitOfWork.GetStudentScheduleByUserIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentScheduleByUserId", pUserId);
                if (!getStudentSchedule.Any())
                {
                    return Response<List<GetStudentScheduleByUserIdDto>>.NoContent();
                }
                var getStudentScheduleByUserIdDto = getStudentSchedule.Select(ss => new GetStudentScheduleByUserIdDto
                {
                    ScheduleDay = ss.ScheduleDay,
                    ScheduleType = ss.ScheduleType,
                    SchedulePlacesName = ss.SchedulePlacesName,
                    Timing = ss.Timing,
                    CoursesName = ss.CoursesName,
                    CoursesCode = ss.CoursesCode,
                    NameDoctor = ss.NameEnglish
                }).ToList();
                return Response<List<GetStudentScheduleByUserIdDto>>.Success(getStudentScheduleByUserIdDto, "The schedule retrieved successfully")
                    .WithCount(getStudentScheduleByUserIdDto.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(GetStudentScheduleByUserIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetStudentScheduleByUserIdDto>>.ServerError("Error occured while retrieving the schedule",
                         "An unexpected error occurred while retrieving the schedule. Please try again later.");
            }
        }

        public async Task<Response<List<GetStudentBySectionIdDto>>> GetStudentBySectionIdAsync(int sectionId)
        {
            try
            {
                SqlParameter pSectionId = new SqlParameter("@ScheduleId", sectionId);

                var getStudent = await _unitOfWork.GetStudentBySectionIdModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetStudentBySectionId", pSectionId);
                if (!getStudent.Any())
                {
                    return Response<List<GetStudentBySectionIdDto>>.NoContent();
                }
                var getStudentBySectionIdDto = getStudent.Select(ss => new GetStudentBySectionIdDto
                {
                    StudentName = ss.NameEnglish,
                    StudentCode = ss.Code
                }).ToList();
                return Response<List<GetStudentBySectionIdDto>>.Success(getStudentBySectionIdDto, "The student retrieved successfully")
                    .WithCount(getStudentBySectionIdDto.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(GetStudentBySectionIdAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<List<GetStudentBySectionIdDto>>.ServerError("Error occured while retrieving the student",
                         "An unexpected error occurred while retrieving the student. Please try again later.");
            }
        }

        public async Task<Response<GetScheduleDetailsDto>> GetScheduleDetailsAsync(int semesterId, int factlyId)
        {
            try
            {
                SqlParameter pSemesterId = new SqlParameter("@ScientificDegreeId", semesterId);
                SqlParameter pFactlyId = new SqlParameter("@FacultyId", factlyId);

                var getSchedulesForSemester = await _unitOfWork.GetScheduleDetailsModels.CallStoredProcedureAsync(
                    "EXECUTE SpGetScheduleDetails", pFactlyId, pSemesterId);
                if (!getSchedulesForSemester.Any())
                {
                    return Response<GetScheduleDetailsDto>.NoContent();
                }
                GetScheduleDetailsDto getScheduleDetailsDto = new GetScheduleDetailsDto
                {
                    AcademyYearName = getSchedulesForSemester.FirstOrDefault().AcademyYear,
                    FacultyName = getSchedulesForSemester.FirstOrDefault().FacultyName,
                    ScientificDegreesName = $"{getSchedulesForSemester.FirstOrDefault().BandName} - {getSchedulesForSemester.FirstOrDefault().ScientificDegreesName}",
                    GetScheduleDetailsInfo = getSchedulesForSemester.Select(s => new GetScheduleDetailsInfoDto
                    {
                        ScheduleDay = s.ScheduleDay,
                        ScheduleType = s.ScheduleType,
                        Timing = s.Timing,
                        Capacity = s.Capacity,
                        CourseCode = s.CourseCode,
                        CourseName = s.CourseName,
                        SchedulePlacesName = s.SchedulePlacesName,
                        StaffName = s.StaffName
                    }).ToList()
                };
                return Response<GetScheduleDetailsDto>.Success(getScheduleDetailsDto, "The schedule retrieved successfully")
                    .WithCount(getScheduleDetailsDto.GetScheduleDetailsInfo.Count);
            }
            catch (Exception ex)
            {
                await _mailService.SendExceptionEmail(new ExceptionEmailModel
                {
                    ClassName = nameof(ScheduleIService),
                    MethodName = nameof(GetScheduleDetailsAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    Time = DateTime.UtcNow
                });
                return Response<GetScheduleDetailsDto>.ServerError("Error occured while retrieving the schedule",
                         "An unexpected error occurred while retrieving the schedule. Please try again later.");
            }
        }
    }
}
