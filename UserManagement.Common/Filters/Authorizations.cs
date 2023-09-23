using System;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace UserManagement.Common.Filters
{
    public class Authorizations : IAuthorizationFilter
    { 
        private readonly IConfiguration _configuration;
        public string UserRole { get; private set; }
        public int RoleId { get; private set; }
        public Authorizations(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            var routeData = context.ActionDescriptor.RouteValues;

            if (routeData.TryGetValue("action", out string actionName) && routeData.TryGetValue("controller", out string controllerName))
            {
                string storedActionName = actionName; // Store the action name in a string variable
                string storedControllerName = controllerName; // Store the controller name in a string variable

                if (storedControllerName.Equals("WeatherForecast", StringComparison.OrdinalIgnoreCase))             
                    return;
                
                // Rest of your existing code
                var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                var IdClaim = user.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                int permissionId = 0;
                if (roleClaim != null)
                {
                    UserRole = roleClaim.Value;
                    if (int.TryParse(IdClaim.Value, out int userId))
                    {
                        RoleId = userId; // Store the user ID as an integer                
                    }
                    permissionId = GetPermissionId(storedActionName, storedControllerName);
                    if (CheckPermissionAuthorization(RoleId, permissionId))
                        return;
                }
                else
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
        }

        private int GetPermissionId(string actionName, string controllerName)
        {
            int permissionId = 0;
           // string connectionString = "Server=localhost;Database=usermanagement;Uid=root;Pwd=12345;";
            using (MySqlConnection connection = new MySqlConnection(_configuration.GetConnectionString("ConEmp")))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("GetPermissionId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Make sure the parameter names match the stored procedure's parameters
                    command.Parameters.Add(new MySqlParameter("@action_name", MySqlDbType.VarChar) { Value = actionName });
                    command.Parameters.Add(new MySqlParameter("@controller_name", MySqlDbType.VarChar) { Value = controllerName });

                    // Add an output parameter for permission_id
                    MySqlParameter permissionIdParam = new MySqlParameter("@permission_id", MySqlDbType.Int32);
                    permissionIdParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(permissionIdParam);

                    // Execute the command
                    command.ExecuteNonQuery();

                    // Check if the output parameter has a value and set permissionId
                    if (permissionIdParam.Value != DBNull.Value)
                    {
                        permissionId = Convert.ToInt32(permissionIdParam.Value);
                    }
                }
            }
            return permissionId;
        }
        private bool CheckPermissionAuthorization(int roleId, int permissionId)
        {
            using (var connection = new MySqlConnection(_configuration.GetConnectionString("ConEmp")))
            {
                connection.Open();
                using (var command = new MySqlCommand("GetPermissionStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@role_id", roleId);
                    command.Parameters.AddWithValue("@permission_id", permissionId);

                    var isAllowedParameter = command.Parameters.Add("@is_allowed", MySqlDbType.Bit);
                    isAllowedParameter.Direction = ParameterDirection.Output;

                    command.ExecuteNonQuery();

                    bool isAllowed = Convert.ToBoolean(isAllowedParameter.Value);
                    return isAllowed;
                }
            }
        }
    }
}
