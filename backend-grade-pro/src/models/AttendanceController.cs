using backend_grade_pro.src.Data;
using backend_grade_pro.src.DTO;
using backend_grade_pro.src.models;
using backend_grade_pro.src.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_grade_pro.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public AttendanceController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Professor")]
        public IActionResult RegisterAttendance([FromBody] Attendance attendance)
        {
            var existingAttendance = _context.Attendances
                .FirstOrDefault(a => a.UserId == attendance.UserId && a.CourseId == attendance.CourseId && a.Date == attendance.Date);

            if (existingAttendance != null)
            {
                return BadRequest("Attendance for this user on this date already exists.");
            }

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Ok(attendance);
        }

        [HttpPost("justify")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> JustifyAbsence([FromForm] JustifyAbsenceDTO justifyAbsenceDTO, IFormFile file)
        {
            var attendance = _context.Attendances
                .FirstOrDefault(a => a.Id == justifyAbsenceDTO.AttendanceId);

            if (attendance == null)
            {
                return NotFound("Attendance record not found.");
            }

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("Justifications", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                attendance.JustificationFileUrl = filePath;
            }

            attendance.Status = "Justified";
            attendance.Justification = justifyAbsenceDTO.Justification;
            _context.SaveChanges();

            // Enviar correo electrónico al profesor
            var professorEmail = _context.Courses
                .Where(c => c.Id == attendance.CourseId)
                .Select(c => c.Tutor.Email)
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(professorEmail))
            {
                _emailService.SendJustificationEmail(professorEmail, attendance.User.FullName, attendance.Date, justifyAbsenceDTO.Justification, attendance.JustificationFileUrl);
            }

            return Ok(attendance);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Student,Professor")]
        public IActionResult GetUserAttendance(int userId)
        {
            var attendances = _context.Attendances
                .Where(a => a.UserId == userId)
                .ToList();

            if (attendances == null || !attendances.Any())
            {
                return NotFound("No attendance records found for the specified user.");
            }

            return Ok(attendances);
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Professor")]
        public IActionResult GetCourseAttendance(int courseId)
        {
            var attendances = _context.Attendances
                .Where(a => a.CourseId == courseId)
                .ToList();

            if (attendances == null || !attendances.Any())
            {
                return NotFound("No attendance records found for the specified course.");
            }

            return Ok(attendances);
        }
    }
}

