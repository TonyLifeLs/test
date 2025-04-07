using backend_grade_pro.src.Data;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_grade_pro.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Student")]
        [Consumes("multipart/form-data")] 
        public async Task<IActionResult> UploadAssignment([FromForm] int courseId, [FromForm] IFormFile file)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value);

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var filePath = Path.Combine("Assignments", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var assignment = new Assignment
            {
                UserId = userId,
                CourseId = courseId,
                FilePath = filePath,
                SubmissionDate = DateTime.Now
            };

            _context.Assignments.Add(assignment);
            _context.SaveChanges();

            return Ok(assignment);
        }

        [HttpPost("grade")]
        [Authorize(Roles = "Professor")]
        public IActionResult GradeAssignment([FromQuery] int assignmentId, [FromBody] GradeAssignmentRequest request)
        {
            var assignment = _context.Assignments.FirstOrDefault(a => a.Id == assignmentId);

            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            assignment.Grade = request.Grade;
            assignment.Comments = request.Comments;
            _context.SaveChanges();

            return Ok(assignment);
        }

        [HttpGet("course/{courseId}")]
        [Authorize(Roles = "Professor")]
        public IActionResult GetAssignmentsByCourse(int courseId)
        {
            var assignments = _context.Assignments
                .Where(a => a.CourseId == courseId)
                .ToList();

            if (assignments == null || !assignments.Any())
            {
                return NotFound("No assignments found for the specified course.");
            }

            return Ok(assignments);
        }
    }

    public class GradeAssignmentRequest
    {
        public double Grade { get; set; }
        public string Comments { get; set; }
    }
}
