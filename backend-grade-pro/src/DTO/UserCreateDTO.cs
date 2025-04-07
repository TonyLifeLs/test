namespace backend_grade_pro.src.DTO
{
    public class UserCreatedto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string SecondLastName { get; set; }
        public string Identity { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public bool IsTutor { get; set; }
        public string Password { get; set; } // Agregado

        public bool IsRepresentant { get; set; }
        public int RoleId { get; set; }
        public int GenderId { get; set; }
        public string Photo { get; set; }
        public string TutorInfoJson { get; set; }
        public ICollection<int> CourseIds { get; set; } // Opcional
        public ICollection<int> QuizIds { get; set; } // Opcional
    }
}
