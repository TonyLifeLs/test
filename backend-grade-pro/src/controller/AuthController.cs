using backend_grade_pro.src.Data;
using backend_grade_pro.src.DTO;
using backend_grade_pro.src.Services;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_grade_pro.src.Helper;

namespace backend_grade_pro.src.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthController(ApplicationDbContext context, IConfiguration configuration, EmailService emailService)
        {
            _context = context;
            _configuration = configuration;
            _emailService = emailService;
        }

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(LoginDTO loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityCard == loginDto.Identity);
            if (user == null || !AuthHelper.VerifyPassword(loginDto.Password, user.Password))
            {
                if (user != null)
                {
                    user.FailedLoginAttempts++;
                    if (user.FailedLoginAttempts >= 3)
                    {
                        user.IsActive = false;
                        _context.AccountLockReasons.Add(new AccountLockReason
                        {
                            UserId = user.Id,
                            Reason = "Intentos fallidos de inicio de sesión",
                            CreatedAt = DateTime.UtcNow
                        });
                        _emailService.SendAccountLockEmail(user.Email, "Intentos fallidos de inicio de sesión");
                    }
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                return Unauthorized();
            }

            if (!user.IsActive)
            {
                return Unauthorized("Cuenta inactiva. Contacte al administrador.");
            }

            user.FailedLoginAttempts = 0;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Generar el token JWT
            var token = AuthHelper.GenerateJwtToken(user, _configuration);
            return Ok(new { Token = token });
        }

        // POST: api/Auth/ForgotPassword
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityCard == forgotPasswordDto.Identity);
            if (user == null)
            {
                return NotFound();
            }

            // Generar y actualizar el token
            user.Token = AuthHelper.GenerateToken();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Enviar el token al correo electrónico del usuario (simulado)
            _emailService.SendForgotPasswordEmail(user.Email, user.Token);

            return Ok(new { Message = "Token enviado al correo electrónico." });
        }

        // POST: api/Auth/ForgotUser
        [HttpPost("ForgotUser")]
        public async Task<IActionResult> ForgotUser(ForgotUserDTO forgotUserDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.IdentityCard == forgotUserDto.Identity);
            if (user == null)
            {
                return NotFound();
            }

            // Generar y actualizar el token
            user.Token = AuthHelper.GenerateToken();
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // Enviar el token al correo electrónico del usuario (simulado)
            _emailService.SendForgotUserEmail(user.Email, user.Token);

            return Ok(new { Message = "Token enviado al correo electrónico." });
        }
      
        // GET: api/Auth/TestToken
        [HttpGet("TestToken")]
        public IActionResult TestToken()
        {
            // Crear un usuario de prueba
            var testUser = new User
            {
                Id = 1,
                IdentityCard = "1726624461",
                Email = "testuser@example.com",
                IsActive = true
            };

            // Generar el token JWT
            var token = AuthHelper.GenerateJwtToken(testUser, _configuration);
            return Ok(new { Token = token });
        }
    }
}
