namespace backend_grade_pro.src.models
{
    public class Course
    {
        public int Id { get; set; }
        public string Parallel { get; set; }
        public bool State { get; set; }
        public int TutorId { get; set; }
        public User Tutor { get; set; }
        public ICollection<UserCourse> UserCourses { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public ICollection<Quiz> Quizzes { get; set; } // Agregado
    }
}
