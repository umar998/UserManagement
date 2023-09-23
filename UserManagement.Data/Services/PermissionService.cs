using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Common.Schema.Models;

namespace UserManagement.Data.Services
{
    public class PermissionService
    {
        private readonly UserContext _context;
        private readonly IConfiguration _configuration;
        public PermissionService(UserContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool PermissionExists(string controller, string action)
        {
            return _context.Permission
                .Any(p => p.Controllername == controller && p.Actionname == action);
        }

        public async Task AddPermission(Permission r)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"CALL AddPermission({r.Actionname}, {r.Controllername})");
        }
        public async Task UpdatePermisssion([FromBody] PermissionUpdateRequest request)
        {
            if (_context != null)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                $"CALL UpdateRolePermission({request.Rolepermissionid}, {request.Isallowed})");
                
            }
            else
            {
                // Handle the case where _context is null
                // You can throw an exception with a custom message
                throw new InvalidOperationException("Database context is not initialized.");
            }
        }

        public async Task AddRolesPermission(RolePermission r)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"CALL AddRolesPermission({r.Permissionid}, {r.Role}, {r.Isallowed})");
        }

        //public async Task<List<AllPermissionWithRoles>> GetAllPermissions()
        //{
        //    var result = await _context.AllPermissionWithRoles
        //        .FromSqlRaw("CALL AllPermissionsWithRoles()")
        //        .ToListAsync();

        //    return result;
        //}

        public async Task<List<AllPermissionWithRoles>> GetAllPermissions()
        {
            string connectionString = _configuration.GetConnectionString("ConEmp"); // Replace with your MySQL connection string
            List<AllPermissionWithRoles> permissions = new List<AllPermissionWithRoles>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (MySqlCommand command = new MySqlCommand("AllPermissionsWithRoles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            int permissionId = reader.GetInt32("permissionid");
                            string actionName = reader.GetString("actionname");
                            string controllerName = reader.GetString("controllername");
                            int rolePermissionId = reader.GetInt32("rolepermissionid");
                            int roleId = reader.GetInt32("roleid");
                            int isAllowed = reader.GetInt32("isallowed");

                            permissions.Add(new AllPermissionWithRoles
                            {
                                Permissionid = permissionId,
                                Actionname = actionName,
                                Controllername = controllerName,
                                Rolepermissionid = rolePermissionId,
                                Roleid = roleId,
                                Isallowed = isAllowed
                            });
                        }
                    }
                }
            }

            return permissions;
        }

        public async Task AddingRolesPermissions()
        {
            var permissions = await _context.Permission.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            foreach (var permission in permissions)
            {
                foreach (var role in roles)
                {
                    // Check if the combination of Roleid and Permissionid already exists
                    bool combinationExists = await _context.RolePermission
                        .AnyAsync(rp => rp.Roleid == role.RoleId && rp.Permissionid == permission.Permissionid);
                    if (!combinationExists)
                    {
                        var rolesPermission = new RolePermission
                        {
                            Permissionid = permission.Permissionid,
                            Roleid = role.RoleId,
                            Isallowed = true // Set as needed
                        };
                        _context.RolePermission.Add(rolesPermission);
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
