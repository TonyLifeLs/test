namespace backend_grade_pro.src.models
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int PermissionId { get; set; }
        public Permission Permission { get; set; }

        public bool State { get; set; }
    }
}
