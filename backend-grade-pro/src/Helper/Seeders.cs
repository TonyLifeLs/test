using backend_grade_pro.src.Data;
using backend_grade_pro.src.models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using backend_grade_pro.src.models.backend_grade_pro.src.models;
using backend_grade_pro.src.middleware;
using backend_grade_pro.src.Services;

namespace backend_grade_pro.src.Helper
{
    public class Seeders
    {
        public static async Task SeedRolesAndPermissions(ApplicationDbContext context)
        {
            if (!context.Roles.Any() && !context.Permissions.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Name = "Representante", State = true },
                    new Role { Name = "Profesor", State = true },
                    new Role { Name = "Estudiante", State = true },
                    new Role { Name = "SuperAdministrador", State = true }
                };

                var permissions = new List<Permission>
                {
                    new Permission { Name = "Puede ver notas", State = true },
                    new Permission { Name = "Puede editar notas", State = true },
                    new Permission { Name = "Puede agregar notas", State = true },
                    new Permission { Name = "Administración total de calificaciones", State = true },
                    new Permission { Name = "Crear preguntas", State = true },
                    new Permission { Name = "Acceso a notas", State = true }
                };

                await context.Roles.AddRangeAsync(roles);
                await context.Permissions.AddRangeAsync(permissions);
                await context.SaveChangesAsync();

                var rolePermissions = new List<RolePermission>
                {
                    new RolePermission { RoleId = roles.First(r => r.Name == "Representante").Id, PermissionId = permissions.First(p => p.Name == "Puede ver notas").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "Profesor").Id, PermissionId = permissions.First(p => p.Name == "Puede ver notas").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "Profesor").Id, PermissionId = permissions.First(p => p.Name == "Puede editar notas").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "Profesor").Id, PermissionId = permissions.First(p => p.Name == "Puede agregar notas").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "SuperAdministrador").Id, PermissionId = permissions.First(p => p.Name == "Administración total de calificaciones").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "SuperAdministrador").Id, PermissionId = permissions.First(p => p.Name == "Crear preguntas").Id, State = true },
                    new RolePermission { RoleId = roles.First(r => r.Name == "SuperAdministrador").Id, PermissionId = permissions.First(p => p.Name == "Acceso a notas").Id, State = true }
                };

                await context.RolePermissions.AddRangeAsync(rolePermissions);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedGenders(ApplicationDbContext context)
        {
            if (!context.Genders.Any())
            {
                var genders = new List<Gender>
                {
                    new Gender { Name = "Masculino" },
                    new Gender { Name = "Femenino" },
                    new Gender { Name = "Prefiero no decirlo" }
                };

                await context.Genders.AddRangeAsync(genders);
                await context.SaveChangesAsync();
            }
        }

        public static async Task SeedPhotos(ApplicationDbContext context)
        {
            if (!context.Photos.Any())
            {
                var photos = new List<Photo>
                {
                    new Photo { Url = "imagen1.png" },
                    new Photo { Url = "imagen2.png" },
                    new Photo { Url = "imagen3.png" }
                };

                await context.Photos.AddRangeAsync(photos);
                await context.SaveChangesAsync();
            }
        }
    }



    namespace backend_grade_pro.src.Helper
    {
        public static class UserSeeder
        {
            public static async Task Seed(ApplicationDbContext context, string jwtSecretKey)
            {
                if (!context.Users.Any())
                {
                    var firstNames = new List<string>
                {
                    "José", "Juan", "Luis", "Carlos", "Jorge", "Manuel", "Antonio", "Pedro", "David", "Alejandro",
                    "Francisco", "Ricardo", "Roberto", "Miguel", "Fernando", "Andrés", "Sergio", "Óscar", "Julio", "Eduardo",
                    "Daniel", "Gabriel", "Martín", "Diego", "Vicente", "Armando", "Raúl", "Ernesto", "Gustavo", "Jaime",
                    "Héctor", "Fabián", "Edwin", "Iván", "Marco", "Mauricio", "Salvador", "Moisés", "Patricio", "Alberto",
                    "Esteban", "Guillermo", "Alfonso", "Mariano", "Rodrigo", "Adrián", "Sebastián", "Matías", "Cristian", "Eloy",
                    "María", "Ana", "Carmen", "Luisa", "Rosa", "Isabel", "Patricia", "Claudia", "Adriana", "Andrea",
                    "Teresa", "Verónica", "Silvia", "Beatriz", "Ángela", "Karla", "Lucía", "Daniela", "Juana", "Sonia",
                    "Elena", "Alicia", "Gabriela", "Mariana", "Carolina", "Fabiola", "Inés", "Mónica", "Rebeca", "Norma",
                    "Pamela", "Cecilia", "Esther", "Lourdes", "Irene", "Rosaura", "Julia", "Antonia", "Raquel", "Nadia",
                    "Rosario", "Victoria", "Sandra", "Fernanda", "Carla", "Lorena", "Mara", "Emilia", "Pilar", "Noelia"
                };

                    var lastNames = new List<string>
                {
                    "García", "Pérez", "Rodríguez", "González", "Martínez", "López", "Sánchez", "Díaz", "Herrera", "Rojas",
                    "Castro", "Morales", "Suárez", "Torres", "Ortiz", "Ramírez", "Jiménez", "Flores", "Mendoza", "Delgado",
                    "Romero", "Cabrera", "Aguirre", "Reyes", "Acosta", "Vargas", "Molina", "Cruz", "Ponce", "Valdez",
                    "Vega", "Escobar", "Fuentes", "Lozano", "Cevallos", "Cordero", "Andrade", "Maldonado", "Salcedo", "Zambrano",
                    "Carrasco", "Quintana", "Ortega", "Naranjo", "Aguayo", "Espinoza", "Barrera", "Caballero", "Cortés", "Páez",
                    "Acosta", "Aguilar", "Alvarado", "Andrade", "Angulo", "Ankuash", "Aguirre", "Aguayo", "Alfonso", "Almeida",
                    "Alvarado", "Alvarez", "Andrade", "Andi", "Arias", "Armijos", "Ballesteros", "Barrera", "Benavides", "Bermúdez",
                    "Bolaños", "Caballero", "Cabrera", "Caicedo", "Calle", "Calvache", "Cando", "Cangá", "Cano", "Cárdenas",
                    "Carrasco", "Carrión", "Cartagena", "Castillo", "Cedeño", "Cevallos", "Chávez", "Chicaiza", "Chiriboga", "Chisaguano",
                    "Cisneros", "Constante", "Cordero", "Cornejo", "Cortés", "Crespo", "Cruz", "Cuji", "Dávila", "Delgado",
                    "Domínguez", "Enríquez", "Espinoza", "Estrella", "Fajardo", "Falconí", "Farías", "Fernández", "Figueroa", "Flores",
                    "Freire", "Gaibor", "Gallegos", "García", "Gavilánez", "Gil", "Gómez", "González", "Gorozabel", "Granda",
                    "Grefa", "Gualinga", "Guamán", "Guerrero", "Guzmán", "Hidalgo", "Hoyos", "Ibarra", "Intriago", "Jaramillo",
                    "Jiménez", "Jimpikit", "Lara", "Lema", "León", "López", "Loor", "Lozano", "Macías", "Maldonado",
                    "Marmol", "Martínez", "Masaquiza", "Medina", "Mendoza", "Minchala", "Molina", "Moncayo", "Montenegro", "Mora"
                };

                    var roles = await context.Roles.ToListAsync();
                    var gender = await context.Genders.FirstAsync(g => g.Name == "Masculino");

                    var users = new List<User>();
                    var random = new Random();

                    for (int i = 0; i < 350; i++)
                    {
                        var firstName = firstNames[random.Next(firstNames.Count)];
                        var secondName = firstNames[random.Next(firstNames.Count)];
                        var lastName = lastNames[i % lastNames.Count];
                        var secondLastName = lastNames[(i + 1) % lastNames.Count];
                        var email = $"{firstName[0].ToString().ToLower()}{lastName.ToLower()}@gmail.com";
                        var passwordHash = BCrypt.Net.BCrypt.HashPassword("123456");
                        var identityCard = EcuadorGenerateIdgenerator.Generate();

                        var role = roles.First(r => r.Name == "Estudiante");
                        if (i < 23)
                        {
                            role = roles.First(r => r.Name == "Profesor");
                        }
                        else if (i < 2)
                        {
                            role = roles.First(r => r.Name == "SuperAdministrador");
                        }
                        else if (i < 25)
                        {
                            role = roles.First(r => r.Name == "Representante");
                        }

                        var user = new User
                        {
                            Name = firstName,
                            SecondName = secondName,
                            LastName = lastName,
                            SecondLastName = secondLastName,
                            Email = email,
                            Password = passwordHash,
                            IsActive = true,
                            RoleId = role.Id,
                            GenderId = gender.Id,
                            IdentityCard = identityCard
                        };

                        user.Token = JwtTokenGenerator.GenerateToken(user, jwtSecretKey);

                        users.Add(user);
                    }

                    await context.Users.AddRangeAsync(users);
                    await context.SaveChangesAsync();
                }

            }



            public static async Task SeedCourses(ApplicationDbContext context)
            {
                if (!context.Courses.Any())
                {
                    var courses = new List<Course>
                {
                    new Course { Parallel = "2A", State = true },
                    new Course { Parallel = "2B", State = true },
                    new Course { Parallel = "2C", State = true },
                    new Course { Parallel = "5F", State = true },
                    new Course { Parallel = "5G", State = true }
                };

                    await context.Courses.AddRangeAsync(courses);
                    await context.SaveChangesAsync();
                }
            }

            public static async Task SeedUserCourses(ApplicationDbContext context)
            {
                if (!context.UserCourses.Any())
                {
                    var users = await context.Users.ToListAsync();
                    var courses = await context.Courses.ToListAsync();
                    var userCourses = new List<UserCourse>();
                    var random = new Random();

                    foreach (var course in courses)
                    {
                        var courseUsers = users.Skip(random.Next(0, users.Count - 35)).Take(35).ToList();
                        foreach (var user in courseUsers)
                        {
                            userCourses.Add(new UserCourse
                            {
                                UserId = user.Id,
                                CourseId = course.Id
                            });
                        }
                    }

                    await context.UserCourses.AddRangeAsync(userCourses);
                    await context.SaveChangesAsync();
                }
            }

            public static async Task SeedQuizzesAndQuestions(ApplicationDbContext context)
            {
                if (!context.Quizzes.Any())
                {
                    var courses = await context.Courses.ToListAsync();
                    var teacher = await context.Users.FirstAsync(u => u.Role.Name == "Profesor");

                    foreach (var course in courses)
                    {
                        var quiz = new Quiz
                        {
                            Title = "Análisis de Sentimientos y Rendimiento Académico",
                            CourseId = course.Id,
                            TeacherId = teacher.Id,
                            State = true,
                            Questions = new List<Question>
                        {
                            new Question
                            {
                                Text = "¿Te sientes motivado para asistir a clases todos los días?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Crees que tus calificaciones reflejan tu verdadero esfuerzo?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Te sientes apoyado por tus profesores en tu proceso de aprendizaje?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Consideras que el ambiente en el aula es positivo y estimulante?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Te sientes estresado por las tareas y exámenes?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "No"
                            },
                            new Question
                            {
                                Text = "¿Crees que recibir retroalimentación positiva mejora tu rendimiento académico?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Te sientes cómodo participando en clase?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Consideras que tus compañeros de clase te apoyan en tu aprendizaje?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Te sientes satisfecho con los recursos educativos proporcionados por la escuela?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            },
                            new Question
                            {
                                Text = "¿Crees que tus emociones afectan tu rendimiento académico?",
                                Options = new List<string> { "Sí", "No" },
                                CorrectAnswer = "Sí"
                            }
                        }
                        };

                        await context.Quizzes.AddAsync(quiz);
                    }

                    await context.SaveChangesAsync();
                }
            }

            public static async Task SeedAnswers(ApplicationDbContext context)
            {
                if (!context.Answers.Any())
                {
                    var students = await context.Users.Where(u => u.Role.Name == "Estudiante").ToListAsync();
                    var questions = await context.Questions.ToListAsync();
                    var random = new Random();

                    var answers = new List<Answer>();

                    foreach (var student in students)
                    {
                        foreach (var question in questions)
                        {
                            var selectedOption = random.Next(2) == 0 ? "Sí" : "No";
                            var isCorrect = selectedOption == question.CorrectAnswer;

                            answers.Add(new Answer
                            {
                                QuestionId = question.Id,
                                UserId = student.Id,
                                SelectedOption = selectedOption,
                                IsCorrect = isCorrect
                            });
                        }
                    }

                    await context.Answers.AddRangeAsync(answers);
                    await context.SaveChangesAsync();
                }
            }
            public static async Task SeedAssignments(ApplicationDbContext context)
            {
                if (!context.Assignments.Any())
                {
                    var students = await context.Users.Where(u => u.Role.Name == "Estudiante").ToListAsync();
                    var teachers = await context.Users.Where(u => u.Role.Name == "Profesor").ToListAsync();
                    var courses = await context.Courses.ToListAsync();
                    var random = new Random();

                    var assignments = new List<Assignment>();

                    foreach (var student in students)
                    {
                        foreach (var course in courses)
                        {
                            var grade = random.NextDouble() * 10; // Genera una calificación entre 0 y 10
                            string comments;

                            if (grade >= 8)
                            {
                                comments = "Excelente trabajo, sigue así!";
                            }
                            else if (grade >= 5)
                            {
                                comments = "Buen trabajo, pero hay áreas que puedes mejorar.";
                            }
                            else
                            {
                                comments = "Necesitas poner más atención y esfuerzo en tus tareas.";
                            }

                            var teacher = teachers[random.Next(teachers.Count)];

                            assignments.Add(new Assignment
                            {
                                UserId = student.Id,
                                CourseId = course.Id,
                                FilePath = $"tarea_{student.Id}_{course.Id}.pdf",
                                SubmissionDate = DateTime.Now.AddDays(-random.Next(1, 30)), // Fecha de entrega aleatoria en los últimos 30 días
                                Grade = grade,
                                Comments = comments,
                                TeacherId = teacher.Id // Asigna el profesor que calificó la tarea
                            });
                        }
                    }

                    await context.Assignments.AddRangeAsync(assignments);
                    await context.SaveChangesAsync();
                }
            }
            public static async Task SeedSubjects(ApplicationDbContext context)
            {
                if (!context.Subjects.Any())
                {
                    var subjects = new List<Subject>
        {
            new Subject { Name = "Lengua y Literatura", State = true },
            new Subject { Name = "Matemática", State = true },
            new Subject { Name = "Ciencias Naturales", State = true },
            new Subject { Name = "Estudios Sociales", State = true },
            new Subject { Name = "Lengua Extranjera (Inglés)", State = true },
            new Subject { Name = "Educación Cultural y Artística", State = true },
            new Subject { Name = "Educación Física", State = true },
            new Subject { Name = "Física", State = true },
            new Subject { Name = "Química", State = true },
            new Subject { Name = "Biología", State = true },
            new Subject { Name = "Historia", State = true },
            new Subject { Name = "Educación para la Ciudadanía", State = true },
            new Subject { Name = "Filosofía", State = true },
            new Subject { Name = "Emprendimiento y Gestión", State = true }
        };

                    await context.Subjects.AddRangeAsync(subjects);
                    await context.SaveChangesAsync();
                }
            }
            public static async Task SeedSubjectRelations(ApplicationDbContext context)
            {
                var subjects = await context.Subjects.ToListAsync();
                var courses = await context.Courses.ToListAsync();
                var students = await context.Users.Where(u => u.Role.Name == "Estudiante").ToListAsync();
                var teachers = await context.Users.Where(u => u.Role.Name == "Profesor").ToListAsync();
                var random = new Random();

                var grades = new List<Grade>();
                var assignments = new List<Assignment>();
                var quizzes = new List<Quiz>();
                var questions = new List<Question>();

                foreach (var course in courses)
                {
                    foreach (var subject in subjects)
                    {
                        foreach (var student in students)
                        {
                            var teacher = teachers[random.Next(teachers.Count)];

                            // Crear calificaciones
                            var grade = new Grade
                            {
                                UserId = student.Id,
                                CourseId = course.Id,
                                SubjectId = subject.Id,
                                TeacherId = teacher.Id,
                                Score = random.NextDouble() * 10,
                                Comments = "Calificación generada automáticamente",
                                State = true
                            };
                            grades.Add(grade);

                            // Crear tareas
                            var assignmentGrade = random.NextDouble() * 10;
                            string comments;

                            if (assignmentGrade >= 8)
                            {
                                comments = "Excelente trabajo, sigue así!";
                            }
                            else if (assignmentGrade >= 5)
                            {
                                comments = "Buen trabajo, pero hay áreas que puedes mejorar.";
                            }
                            else
                            {
                                comments = "Necesitas poner más atención y esfuerzo en tus tareas.";
                            }

                            var assignment = new Assignment
                            {
                                UserId = student.Id,
                                CourseId = course.Id,
                                FilePath = $"tarea_{student.Id}_{course.Id}.pdf",
                                SubmissionDate = DateTime.Now.AddDays(-random.Next(1, 30)),
                                Grade = assignmentGrade,
                                Comments = comments,
                                TeacherId = teacher.Id
                            };
                            assignments.Add(assignment);

                            // Crear cuestionarios y preguntas
                            var quiz = new Quiz
                            {
                                Title = $"Cuestionario de {subject.Name}",
                                CourseId = course.Id,
                                TeacherId = teacher.Id,
                                State = true
                            };
                            quizzes.Add(quiz);

                            var quizQuestions = new List<Question>
                {
                    new Question
                    {
                        Text = $"Pregunta 1 de {subject.Name}",
                        Options = new List<string> { "Opción 1", "Opción 2", "Opción 3", "Opción 4" },
                        CorrectAnswer = "Opción 1",
                        Quiz = quiz
                    },
                    new Question
                    {
                        Text = $"Pregunta 2 de {subject.Name}",
                        Options = new List<string> { "Opción 1", "Opción 2", "Opción 3", "Opción 4" },
                        CorrectAnswer = "Opción 2",
                        Quiz = quiz
                    }
                };
                            questions.AddRange(quizQuestions);
                        }
                    }
                }

                await context.Grades.AddRangeAsync(grades);
                await context.Assignments.AddRangeAsync(assignments);
                await context.Quizzes.AddRangeAsync(quizzes);
                await context.Questions.AddRangeAsync(questions);
                await context.SaveChangesAsync();
            }
            public static async Task SeedAttendances(ApplicationDbContext context)
            {
                if (!context.Attendances.Any())
                {
                    var students = await context.Users.Where(u => u.Role.Name == "Estudiante").ToListAsync();
                    var teachers = await context.Users.Where(u => u.Role.Name == "Profesor").ToListAsync();
                    var courses = await context.Courses.ToListAsync();
                    var random = new Random();

                    var attendances = new List<Attendance>();
                    var startDate = new DateTime(2024, 10, 1);
                    var endDate = DateTime.Now;

                    foreach (var student in students)
                    {
                        foreach (var course in courses)
                        {
                            var teacher = teachers[random.Next(teachers.Count)];
                            var currentDate = startDate;
                            var absences = 0;

                            while (currentDate <= endDate)
                            {
                                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    var status = "Present";
                                    if (absences < 3 && random.Next(10) < 2) // 20% de probabilidad de faltar
                                    {
                                        status = "Absent";
                                        absences++;
                                    }

                                    attendances.Add(new Attendance
                                    {
                                        UserId = student.Id,
                                        CourseId = course.Id,
                                        Date = currentDate,
                                        Status = status,
                                        TeacherId = teacher.Id
                                    });
                                }
                                currentDate = currentDate.AddDays(1);
                            }
                        }
                    }

                    await context.Attendances.AddRangeAsync(attendances);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

