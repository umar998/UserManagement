using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UserManagement.Common.Schema.Models;
using UserManagement.Data.Services;

namespace UserManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                // Inject required services
                var permissionService = serviceProvider.GetRequiredService<PermissionService>();

                // List of controllers and their actions
                var controllerActions = new List<(string Controller, string Action)>
                {
                      // Add tuples for each controller-action pair
                    ("Role", "AddRole"),
                    ("Role", "GetAllRoles"),
                    ("Permissions", "AddPermissions"),
                    ("Permissions", "AddRolesPermissions"),
                    ("Permissions", "GetAllPermissions"),
                    ("Permissions", "UpdatePermission"),
                    ("Users", "LoginSP"),
                    ("Users", "AddUser"),
                    ("Users", "UpdateUserWithRoles"),
                    ("Users", "AssignRoleToUser"),
                    ("Users", "GetUserById"),
                    ("Users", "DeleteUserAndRoles"),
                    ("Users", "GetAllUsers"),
                    

                };

                foreach (var (controller, action) in controllerActions)
                {
                    // Check if the permission already exists
                    if (!permissionService.PermissionExists(controller, action))
                    {
                        var permission = new Permission
                        {
                            Controllername = controller,
                            Actionname = action
                        };

                        // Add the permission to the database
                        await permissionService.AddPermission(permission);
                    }
                }
                await permissionService.AddingRolesPermissions();
            }

            await host.RunAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
