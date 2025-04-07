using backend_grade_pro.src.Data;
using backend_grade_pro.src.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend_grade_pro.src.controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePermissionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RolePermission>>> GetRolePermissions()
        {
            return await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .ToListAsync();
        }

        [HttpGet("{roleId}/{permissionId}")]
        public async Task<ActionResult<RolePermission>> GetRolePermission(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .Include(rp => rp.Role)
                .Include(rp => rp.Permission)
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
            {
                return NotFound();
            }

            return rolePermission;
        }

        [HttpPost]
        public async Task<ActionResult<RolePermission>> PostRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRolePermission", new { roleId = rolePermission.RoleId, permissionId = rolePermission.PermissionId }, rolePermission);
        }

        [HttpPut("Inactivate/{roleId}/{permissionId}")]
        public async Task<IActionResult> InactivateRolePermission(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

            if (rolePermission == null)
            {
                return NotFound();
            }

            rolePermission.State = false;
            _context.Entry(rolePermission).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
