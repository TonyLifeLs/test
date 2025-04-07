namespace backend_grade_pro.src.models
{

    public class Attendance
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } // "Present", "Absent", "Justified", "Unjustified"
        public string Justification { get; set; } // Nueva propiedad para la justificación
        public string JustificationFileUrl { get; internal set; }
    }
}
