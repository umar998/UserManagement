using Microsoft.EntityFrameworkCore;
using UserManagement.Common.Schema.Models;

namespace UserManagement.Data.Services
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserwithRoleResponse> UserWithRoleResponses { get; set; }
        public DbSet<UpdateUserWithRole> UpdateUserWithRoles { get; set; }
        public DbSet<AllPermissionWithRoles> AllPermissionWithRoles { get; set; }
        //public DbSet<PermissionUpdateRequest> AllPermissionWithRoles { get; set; }

    }
}
