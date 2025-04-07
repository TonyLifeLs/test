namespace backend_grade_pro.src.models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string SelectedOption { get; set; }
        public bool IsCorrect { get; set; }
    }
}
