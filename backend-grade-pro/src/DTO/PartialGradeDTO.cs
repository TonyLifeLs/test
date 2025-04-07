namespace backend_grade_pro.src.DTO
{
    public class PartialGradeDTO
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public int SubjectId { get; set; }
        public double HomeworkScore { get; set; }
        public double ExamScore { get; set; }
    }
}
