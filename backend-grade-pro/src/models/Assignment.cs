namespace backend_grade_pro.src.models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string FilePath { get; set; }
        public DateTime SubmissionDate { get; set; }
        public double? Grade { get; set; }
        public string Comments { get; set; }
    }
}
