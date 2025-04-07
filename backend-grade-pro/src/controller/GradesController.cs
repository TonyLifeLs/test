using backend_grade_pro.src.Data;
using backend_grade_pro.src.DTO;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_grade_pro.src.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GradesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Grades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Grade>>> GetGrades()
        {
            return await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Course)
                .Include(g => g.Subject)
                .Include(g => g.Teacher)
                .ToListAsync();
        }

        // GET: api/Grades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Grade>> GetGrade(int id)
        {
            var grade = await _context.Grades
                .Include(g => g.User)
                .Include(g => g.Course)
                .Include(g => g.Subject)
                .Include(g => g.Teacher)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (grade == null)
            {
                return NotFound();
            }

            return grade;
        }

        // POST: api/Grades
        [HttpPost]
        public async Task<ActionResult<Grade>> PostGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrade", new { id = grade.Id }, grade);
        }

        // PUT: api/Grades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrade(int id, Grade grade)
        {
            if (id != grade.Id)
            {
                return BadRequest();
            }

            _context.Entry(grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Grades/Inactivate/5
        [HttpPut("Inactivate/{id}")]
        public async Task<IActionResult> InactivateGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }

            grade.State = false;
            _context.Entry(grade).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("register")]
        public IActionResult RegisterPartialGrade([FromBody] PartialGradeDTO partialGradeDTO)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.UserId == partialGradeDTO.UserId && g.CourseId == partialGradeDTO.CourseId && g.SubjectId == partialGradeDTO.SubjectId);

            if (grade == null)
            {
                grade = new Grade
                {
                    UserId = partialGradeDTO.UserId,
                    CourseId = partialGradeDTO.CourseId,
                    SubjectId = partialGradeDTO.SubjectId,
                    PartialGrades = new List<PartialGrade>()
                };
                _context.Grades.Add(grade);
            }

            var partialGrade = new PartialGrade
            {
                Grade = grade,
                HomeworkScore = partialGradeDTO.HomeworkScore,
                ExamScore = partialGradeDTO.ExamScore
            };

            grade.PartialGrades.Add(partialGrade);
            _context.SaveChanges();

            return Ok(grade);
        }

        [HttpGet("calculate/{userId}/{courseId}/{subjectId}")]
        public IActionResult CalculateFinalScore(int userId, int courseId, int subjectId)
        {
            var grade = _context.Grades
                .FirstOrDefault(g => g.UserId == userId && g.CourseId == courseId && g.SubjectId == subjectId);

            if (grade == null || grade.PartialGrades == null || grade.PartialGrades.Count == 0)
            {
                return NotFound("No grades found for the specified user, course, and subject.");
            }

            var finalScore = grade.CalculateFinalScore();
            return Ok(finalScore);
        }
    
private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }

}
}
