using GraduationProject.Repository.Repository;

namespace GraduationProject.Service.Service
{
    public class FormatStudentCodeClass
    {
        private readonly UnitOfWork _unitOfWork;
        public FormatStudentCodeClass(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<string> AcademyYear_FacultyId_Increment(int facutlyId)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string studentCode = academyYear + facultyId + Id;
            return studentCode;
        }
        public async Task<string> FacultyId_AcademyYear_Increment(int facutlyId)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string studentCode = facultyId + academyYear + Id;
            return studentCode;
        }
        public async Task<string> FacultyId_AcademyYear_NaID_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = facultyId + academyYear + nId + Id;
            return studentCode;
        }
        public async Task<string> AcademyYear_FacultyId_NaID_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = academyYear + facultyId + nId + Id;
            return studentCode;
        }
        public async Task<string> AcademyYear_NaID_FacultyId_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = academyYear + nId + facultyId + Id;
            return studentCode;
        }
        public async Task<string> NaID_AcademyYear_FacultyId_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = nId + academyYear + facultyId + Id;
            return studentCode;
        }
        public async Task<string> NaID_FacultyId_AcademyYear_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = nId + facultyId + academyYear + Id;
            return studentCode;
        }
        public async Task<string> FacultyId_NaID_AcademyYear_Increment(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string Id;
            var students = await _unitOfWork.Students.GetEntityByPropertyAsync(f => f.FacultyId == facutlyId);
            if (!students.Any())
            {
                Id = "1000";
            }
            else
            {
                var studentWithLargestCode = students.OrderByDescending(s => ExtractLastFourDigits(s.Code, 4)).FirstOrDefault();
                if (studentWithLargestCode == null)
                {
                    return null;
                }
                int lastFouurstudentWithLargestCode = ExtractLastFourDigits(studentWithLargestCode.Code, 4);
                if (lastFouurstudentWithLargestCode == 0)
                {
                    return null;
                }
                lastFouurstudentWithLargestCode++;
                Id = lastFouurstudentWithLargestCode.ToString();
            }
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = facultyId + nId + academyYear + Id;
            return studentCode;
        }
        public async Task<string> AcademyYear_FacultyId(int facutlyId)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string studentCode = academyYear + facultyId;
            return studentCode;
        }
        public async Task<string> FacultyId_AcademyYear(int facutlyId)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string studentCode = facultyId + academyYear;
            return studentCode;
        }
        public async Task<string> FacultyId_AcademyYear_NaID(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = facultyId + academyYear + nId;
            return studentCode;
        }
        public async Task<string> AcademyYear_FacultyId_NaID(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = academyYear + facultyId + nId;
            return studentCode;
        }
        public async Task<string> AcademyYear_NaID_FacultyId(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = academyYear + nId + facultyId;
            return studentCode;
        }
        public async Task<string> NaID_AcademyYear_FacultyId(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = nId + academyYear + facultyId;
            return studentCode;
        }
        public async Task<string> NaID_FacultyId_AcademyYear(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }
            string studentCode = nId + facultyId + academyYear;
            return studentCode;
        }
        public async Task<string> FacultyId_NaID_AcademyYear(int facutlyId, string naID)
        {
            string academyYear = DateTime.UtcNow.Year.ToString();
            string facultyId = facutlyId.ToString();
            string nId = ExtractLastFourDigitsNId(naID, 7);
            if (nId == null)
            {
                return null;
            }

            string studentCode = facultyId + nId + academyYear;
            return studentCode;
        }
        public async Task<string> NaID(string naID)
        {

            if (naID == null)
            {
                return null;
            }
            string studentCode = naID;
            return studentCode;
        }

        private int ExtractLastFourDigits(string code, int length)
        {
            if (string.IsNullOrWhiteSpace(code) || code.Length < length)
                return 0;

            var lastFourDigits = code.Substring(code.Length - length);
            return int.TryParse(lastFourDigits, out var result) ? result : 0;
        }
        private string ExtractLastFourDigitsNId(string nId, int length)
        {
            if (string.IsNullOrWhiteSpace(nId) || nId.Length < length)
                return null;

            var lastFourDigits = nId.Substring(nId.Length - length);
            return lastFourDigits;
        }

    }
}
