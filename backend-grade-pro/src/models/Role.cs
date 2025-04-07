namespace backend_grade_pro.src.models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
