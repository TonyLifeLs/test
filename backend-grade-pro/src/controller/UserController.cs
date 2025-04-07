using backend_grade_pro.src.Data;
using backend_grade_pro.src.DTO;
using backend_grade_pro.src.Services;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using backend_grade_pro.src.Helper;

namespace backend_grade_pro.src.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public UsersController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Gender)
                .Include(u => u.UserCourses)
                .ThenInclude(uc => uc.Course)
                .Include(u => u.Answers)
                .ThenInclude(a => a.Question)
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.Gender)
                .Include(u => u.UserCourses)
                .ThenInclude(uc => uc.Course)
                .Include(u => u.Answers)
                .ThenInclude(a => a.Question)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserCreatedto userDto)
        {
            var password = GenerateRandomPassword();
            if (!ValidatePassword(password))
            {
                return BadRequest("La contraseña debe contener al menos una letra, un número y un carácter especial.");
            }

            var user = new User
            {
                Name = userDto.Name,
                LastName = userDto.LastName,
                SecondName = userDto.SecondName,
                SecondLastName = userDto.SecondLastName,
                Identity = userDto.Identity,
                Email = userDto.Email,
                Phone = userDto.Phone,
                Age = userDto.Age,
                IsTutor = userDto.IsTutor,
                IsRepresentant = userDto.IsRepresentant,
                RoleId = userDto.RoleId,
                GenderId = userDto.GenderId,
                Photo = userDto.Photo,
                TutorInfoJson = userDto.TutorInfoJson,
                Password = AuthHelper.HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Guardar el historial de contraseñas
            _context.PasswordHistories.Add(new PasswordHistory
            {
                UserId = user.Id,
                PasswordHash = user.Password,
                CreatedAt = DateTime.UtcNow
            });

            // Asignar cursos opcionales
            if (userDto.CourseIds != null)
            {
                foreach (var courseId in userDto.CourseIds)
                {
                    _context.UserCourses.Add(new UserCourse { UserId = user.Id, CourseId = courseId });
                }
            }

            // Asignar quizzes opcionales
            if (userDto.QuizIds != null)
            {
                foreach (var quizId in userDto.QuizIds)
                {
                    _context.Answers.Add(new Answer { UserId = user.Id, QuestionId = quizId });
                }
            }

            await _context.SaveChangesAsync();

            // Enviar correo electrónico de bienvenida
            _emailService.SendWelcomeEmail(user.Email, user.Name, password);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserUpdateDTO userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!ValidatePassword(userDto.Password))
            {
                return BadRequest("La contraseña debe contener al menos una letra, un número y un carácter especial.");
            }

            // Verificar si la nueva contraseña ya ha sido utilizada
            var passwordHistory = await _context.PasswordHistories
                .Where(ph => ph.UserId == id)
                .OrderByDescending(ph => ph.CreatedAt)
                .FirstOrDefaultAsync();

            if (passwordHistory != null && AuthHelper.VerifyPassword(userDto.Password, passwordHistory.PasswordHash))
            {
                return BadRequest("No se puede reutilizar la contraseña anterior.");
            }

            user.Name = userDto.Name;
            user.LastName = userDto.LastName;
            user.SecondName = userDto.SecondName;
            user.SecondLastName = userDto.SecondLastName;
            user.Identity = userDto.Identity;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;
            user.Age = userDto.Age;
            user.IsTutor = userDto.IsTutor;
            user.IsRepresentant = userDto.IsRepresentant;
            user.RoleId = userDto.RoleId;
            user.GenderId = userDto.GenderId;
            user.Photo = userDto.Photo;
            user.TutorInfoJson = userDto.TutorInfoJson;
            user.Password = AuthHelper.HashPassword(userDto.Password);

            _context.Entry(user).State = EntityState.Modified;

            // Guardar el historial de contraseñas
            _context.PasswordHistories.Add(new PasswordHistory
            {
                UserId = user.Id,
                PasswordHash = user.Password,
                CreatedAt = DateTime.UtcNow
            });

            // Actualizar cursos opcionales
            if (userDto.CourseIds != null)
            {
                var existingCourses = _context.UserCourses.Where(uc => uc.UserId == id).ToList();
                _context.UserCourses.RemoveRange(existingCourses);

                foreach (var courseId in userDto.CourseIds)
                {
                    _context.UserCourses.Add(new UserCourse { UserId = user.Id, CourseId = courseId });
                }
            }

            // Actualizar quizzes opcionales
            if (userDto.QuizIds != null)
            {
                var existingAnswers = _context.Answers.Where(a => a.UserId == id).ToList();
                _context.Answers.RemoveRange(existingAnswers);

                foreach (var quizId in userDto.QuizIds)
                {
                    _context.Answers.Add(new Answer { UserId = user.Id, QuestionId = quizId });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        private bool ValidatePassword(string password)
        {
            var hasLetter = new Regex(@"[a-zA-Z]+");
            var hasDigit = new Regex(@"[0-9]+");
            var hasSpecialChar = new Regex(@"[\W]+");

            return hasLetter.IsMatch(password) && hasDigit.IsMatch(password) && hasSpecialChar.IsMatch(password);
        }

        private string GenerateRandomPassword()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}

