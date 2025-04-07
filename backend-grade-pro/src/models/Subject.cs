using System.Diagnostics;

namespace backend_grade_pro.src.models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
