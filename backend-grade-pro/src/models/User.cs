using System.Text.Json;
using backend_grade_pro.src.models.backend_grade_pro.src.models;

namespace backend_grade_pro.src.models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string SecondLastName { get; set; }
        public string FullName
        {
            get
            {
                return $"{Name} {SecondName} {LastName} {SecondLastName}";
            }
        }
        public string Identity { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public bool IsTutor { get; set; }
        public bool IsRepresentant { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int GenderId { get; set; }
        public Gender Gender { get; set; }

        public string Photo { get; set; }
        public string TutorInfoJson { get; set; }

        public TutorInfo TutorInfo
        {
            get => string.IsNullOrEmpty(TutorInfoJson) ? null : JsonSerializer.Deserialize<TutorInfo>(TutorInfoJson);
            set => TutorInfoJson = JsonSerializer.Serialize(value);
        }

        public string Password { get; set; }
        public string Token { get; set; }
        public int FailedLoginAttempts { get; set; } // Agregado
        public bool IsActive { get; set; } = true; // Agregado

        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<PasswordHistory> PasswordHistories { get; set; }
        public ICollection<AccountLockReason> AccountLockReasons { get; set; } // Agregado
    }

    public class TutorInfo
    {
        public bool IsTutor { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
