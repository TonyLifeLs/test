namespace backend_grade_pro.src.models
{
    public class PartialGrade
    {
            public int Id { get; set; } 
            public int UserId { get; set; }
            public int CourseId { get; set; }
            public int SubjectId { get; set; }
            public double HomeworkScore { get; set; }
            public double ExamScore { get; set; }
        public Grade Grade { get; internal set; }
        public double TotalScore { get; internal set; }
    }
}
