namespace backend_grade_pro.src.models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
