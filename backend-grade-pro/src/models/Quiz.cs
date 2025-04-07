namespace backend_grade_pro.src.models
{
    public class Quiz
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int TeacherId { get; set; }
        public User Teacher { get; set; }
        public bool State { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
