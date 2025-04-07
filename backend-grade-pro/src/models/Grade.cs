namespace backend_grade_pro.src.models
{
    public class Grade
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public int TeacherId { get; set; }
        public User Teacher { get; set; }

        public double Score { get; set; }
        public string Comments { get; set; }
        public bool State { get; set; }

        public ICollection<PartialGrade> PartialGrades { get; set; }

        public double CalculateFinalScore()
        {
            if (PartialGrades == null || PartialGrades.Count == 0)
                return 0;

            return PartialGrades.Average(pg => pg.TotalScore);
        }
    }
}
