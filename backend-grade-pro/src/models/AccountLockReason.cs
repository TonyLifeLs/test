namespace backend_grade_pro.src.models
{
    public class AccountLockReason
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
