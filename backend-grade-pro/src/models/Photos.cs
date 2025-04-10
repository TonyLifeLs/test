namespace backend_grade_pro.src.models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int UserId { get; set; } // Clave foránea
        public User User { get; set; } // Navegación
    }
}