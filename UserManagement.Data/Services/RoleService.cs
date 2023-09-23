using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Common.Schema.Models;

namespace UserManagement.Data.Services
{
    public class RoleService
    {
        private readonly UserContext _context;
        public RoleService(UserContext context)
        {
            _context = context;
        }

        public async Task AddRole(Roles r)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL AddRole({r.Role})");
        }

        public async Task<List<Roles>> GetAllRoles()
        {
            var result = await _context.Roles
                .FromSqlRaw("CALL GetAllRoles()")
                .ToListAsync();
            return result;
        }
    }
}
