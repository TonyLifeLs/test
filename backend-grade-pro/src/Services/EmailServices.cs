using System.Net;
using System.Net.Mail;

namespace backend_grade_pro.src.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendWelcomeEmail(string toEmail, string userName, string password)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "Bienvenido a Nuestra Plataforma",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #ffffff; border-radius: 10px;'>
                        <h2 style='color: #007bff;'>Bienvenido, {userName}!</h2>
                        <p>Gracias por registrarte en nuestra plataforma. Tu cuenta ha sido creada exitosamente.</p>
                        <p>Tu contraseña temporal es: <strong>{password}</strong></p>
                        <p>Por favor, cambia tu contraseña después de iniciar sesión.</p>
                        <p>Saludos,<br>El equipo de Nuestra Plataforma</p>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

        public void SendAccountLockEmail(string toEmail, string reason)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "Cuenta Inactiva",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #ffffff; border-radius: 10px;'>
                        <h2 style='color: #007bff;'>Cuenta Inactiva</h2>
                        <p>Tu cuenta ha sido inactivada por el siguiente motivo: <strong>{reason}</strong></p>
                        <p>Por favor, contacta al administrador para más información.</p>
                        <p>Saludos,<br>El equipo de Nuestra Plataforma</p>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

        public void SendForgotPasswordEmail(string toEmail, string token)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "Recuperación de Contraseña",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #ffffff; border-radius: 10px;'>
                        <h2 style='color: #007bff;'>Recuperación de Contraseña</h2>
                        <p>Utiliza el siguiente token para recuperar tu contraseña: <strong>{token}</strong></p>
                        <p>Saludos,<br>El equipo de Nuestra Plataforma</p>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }

        public void SendForgotUserEmail(string toEmail, string token)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "Recuperación de Usuario",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #ffffff; border-radius: 10px;'>
                        <h2 style='color: #007bff;'>Recuperación de Usuario</h2>
                        <p>Utiliza el siguiente token para recuperar tu usuario: <strong>{token}</strong></p>
                        <p>Saludos,<br>El equipo de Nuestra Plataforma</p>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
        public void SendJustificationEmail(string toEmail, string studentName, DateTime date, string justification, string fileUrl)
        {
            var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
            {
                Port = int.Parse(_configuration["Smtp:Port"]),
                Credentials = new NetworkCredential(_configuration["Smtp:Username"], _configuration["Smtp:Password"]),
                EnableSsl = bool.Parse(_configuration["Smtp:EnableSsl"])
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Smtp:Username"]),
                Subject = "Justificación de Falta",
                Body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f0f8ff; color: #333;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; background-color: #ffffff; border-radius: 10px;'>
                        <h2 style='color: #007bff;'>Justificación de Falta</h2>
                        <p>El estudiante <strong>{studentName}</strong> ha justificado su falta del día <strong>{date.ToShortDateString()}</strong>.</p>
                        <p>Justificación: <strong>{justification}</strong></p>
                        <p>Archivo de justificación: <a href='{fileUrl}'>Ver archivo</a></p>
                        <p>Saludos,<br>El equipo de Nuestra Plataforma</p>
                    </div>
                </body>
                </html>",
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            smtpClient.Send(mailMessage);
        }
    }
}

