using GraduationProject.Data.Entity;
using GraduationProject.Data.Enum;
using GraduationProject.EntityFramework.DataBaseContext;
using GraduationProject.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject.Repository.Repository
{
    public class SchedulesRepository : GeneralRepository<Schedule>, ISchedulesRepository
    {
        private readonly ApplicationDbContext _context;
        public SchedulesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AssignStudentsToSchedule(int ScientificDegree)
        {
            try
            {
                var studentSemesterWithCourses = await _context.StudentSemesters.Where(ss => ss.ScientificDegreeId == ScientificDegree && ss.AcademyYear.IsCurrent)
                    .Include(ss => ss.StudentSemesterCourse)
                    //.Include(ss=>ss.Student)
                    //    .ThenInclude(s=>s.User)
                    .ToListAsync();

                var schedules = await _context.Schedules.Where(s => s.ScientificDegreeId == ScientificDegree && s.AcademyYear.IsCurrent).ToListAsync();

                //var sortedStudentSemesterWithCourses = studentSemesterWithCourses
                //    .OrderBy(std => std.Student.User.NameArabic)
                //    .ToList();

                foreach (var student in studentSemesterWithCourses)
                {
                    var studentScheduleList = new List<StudentSchedule>();
                    var studentCurrentSchedules = new List<Schedule>();
                    foreach (var course in student.StudentSemesterCourse)
                    {
                        var courseSchedules = schedules.Where(s => s.CourseId == course.CourseId).ToList();
                        if (courseSchedules.Count == 0)
                        {
                            continue;
                        }
                        var lectures = courseSchedules.Where(s => s.ScheduleType == ScheduleType.Lecture).ToList();
                        var sections = courseSchedules.Where(s => s.ScheduleType == ScheduleType.Section).ToList();

                        if (!lectures.Any()) { continue; }
                        bool lecturesFlag = false;
                        foreach (var lecture in lectures)
                        {
                            if (IsScheduleRejected(studentCurrentSchedules, lecture))
                            {
                                continue;
                            }
                            if (lecture.CurrentCapacity == null)
                            {
                                lecture.CurrentCapacity = 0;
                            }
                            if (lecture.Capacity == lecture.CurrentCapacity)
                            {
                                continue;
                            }
                            studentScheduleList.Add(new StudentSchedule { StudentId = student.StudentId, ScheduleId = lecture.Id });
                            studentCurrentSchedules.Add(lecture);
                            lecture.CurrentCapacity++;
                            lecturesFlag = true;
                            break;
                        }
                        if (!lecturesFlag) { continue; }
                        foreach (var section in sections)
                        {
                            if (IsScheduleRejected(studentCurrentSchedules, section))
                            {
                                continue;
                            }
                            if (section.CurrentCapacity == null)
                            {
                                section.CurrentCapacity = 0;
                            }
                            if (section.Capacity == section.CurrentCapacity)
                            {
                                continue;
                            }
                            studentScheduleList.Add(new StudentSchedule { StudentId = student.StudentId, ScheduleId = section.Id });
                            studentCurrentSchedules.Add(section);
                            section.CurrentCapacity++;
                            break;
                        }
                    }
                    await _context.StudentSchedules.AddRangeAsync(studentScheduleList);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        private bool IsScheduleRejected(List<Schedule> oldSchedules, Schedule newSchedule)
        {
            if (!oldSchedules.Any())
            {
                return false;
            }

            var newTimeStart = newSchedule.TimeStart;
            var newTimeEnd = newSchedule.EndStart;

            var conflictingSchedules = oldSchedules
                .Where(s => s.ScheduleDay == newSchedule.ScheduleDay)
                .ToList();

            return IsRangeRejected(newTimeStart, newTimeEnd, conflictingSchedules.Select(s => (s.TimeStart, s.EndStart)).ToList());
        }

        private bool IsRangeRejected(TimeSpan inputStart, TimeSpan inputEnd, List<(TimeSpan start, TimeSpan end)> ranges)
        {
            foreach (var range in ranges)
            {
                if (inputStart < range.end && inputEnd > range.start)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
