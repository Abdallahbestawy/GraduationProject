using GraduationProject.Identity.IService;
using GraduationProject.Service.DataTransferObject.StudentDto;
using GraduationProject.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IAccountService _accountService;

        public StudentController(IStudentService studentService, IAccountService accountService)
        {
            _studentService = studentService;
            _accountService = accountService;
        }

        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(AddStudentDto addStudentDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _studentService.AddStudentAsync(addStudentDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter valid Model");
            }
        }

        [HttpPost("AddStudentSemester")]
        public async Task<IActionResult> AddStudentSemester([FromBody] AddStudentSemesterDto addStudentSemesterDto)
        {
            if (ModelState.IsValid)
            {
                var response = await _studentService.AddStudentSemesterAsync(addStudentSemesterDto);

                return StatusCode(response.StatusCode, response);
            }
            else
            {
                return BadRequest("please enter Vaild Model");
            }
        }
        [Authorize(Roles = "Student")]
        [HttpGet("BasicData")]
        public async Task<IActionResult> GetStudent()
        {
            var currentUser = await _accountService.GetUser(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var response = await _studentService.GetStudentByUserId(currentUser.Id);

            return StatusCode(response.StatusCode, response);

        }

        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            var response = await _studentService.GetAllStudentsAsync();

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("student{studentId:int}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            if (studentId == 0 || studentId == null)
            {
                return BadRequest("please enter valid StudentId");
            }
            var respone = await _studentService.DeleteStudentAsync(studentId);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpDelete("studentSemesters{studentSemesterId:int}")]
        public async Task<IActionResult> DeleteStudentSemester(int studentSemesterId)
        {
            if (studentSemesterId == 0 || studentSemesterId == null)
            {
                return BadRequest("please enter valid studentSemesterId");
            }
            var respone = await _studentService.DeleteStudentSemesterAsync(studentSemesterId);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentDto updateStudentDto)
        {
            if (updateStudentDto == null)
            {
                return BadRequest("Please enter valid Model");
            }
            var respone = await _studentService.UpdateStudentAsync(updateStudentDto);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpGet("Result")]
        public async Task<IActionResult> GetResultStudent()
        {
            string userId = "af88e91d-7241-4149-bbd4-ebb2a30dd247";
            var respone = await _studentService.GetStudentResultAsync(userId);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpGet("as{semesterId:int}")]
        public async Task<IActionResult> GetAllStudentsInSemester(int semesterId)
        {
            var respone = await _studentService.GetAllStudentsInSemesterAsync(semesterId);

            return StatusCode(respone.StatusCode, respone);
        }

        [HttpGet("InfoData{studentId:int}")]
        public async Task<IActionResult> GetStudentInfo(int studentId)
        {
            var response = await _studentService.GetStudentInfoByStudentIdAsync(studentId);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("AddListOfStudentsFromExcel")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddStudentsListFromExcel(IFormFile file)
        {
            if (Request.Form.Files.Count == 0) return BadRequest("No files are uploaded");

            //var file = Request.Form.Files[0];

            var response = await _studentService.AddStudentsListFromExcelFileAsync(file, User);

            return StatusCode(response.StatusCode, response);
        }
    }
}
