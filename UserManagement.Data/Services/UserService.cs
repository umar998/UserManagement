using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Common.Schema.Models;

namespace UserManagement.Data.Services
{
    public class UserService
    {
        private readonly UserContext _context;
        private readonly ILogger<UserService> _logger;
        public UserService(UserContext context,ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task AddUser(User r)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                $"CALL AddUser({r.Name}, {r.Age}, {r.Password})");
        }

        public async Task DeleteUserAndRoles(int userId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL DeleteUserAndRoles({userId})");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the database operation
                throw ex;
            }
        }
        public async Task UpdateUserWithRole(UpdateUserWithRole r)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL UpdateUserWithRole({r.Userid},{r.Name}, {r.Age}, {r.Password},{r.Roleid})");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error executing SQL command: {ex.Message}");
                throw; // Re-throw the exception to propagate it up the call stack
            }
        }

        //public int UpdateUserWithRole(int userId, string newName, int newAge, string newPassword, int newRoleId)
        //{
        //    var parameters = new[]
        //    {
        //        new MySqlParameter("@userIdd", userId),
        //        new MySqlParameter("@newName", newName),
        //        new MySqlParameter("@newAge", newAge),
        //        new MySqlParameter("@newPassword", newPassword),
        //        new MySqlParameter("@newRoleId", newRoleId)
        //    };

        //    return _context.Database.ExecuteSqlRaw("CALL UpdateUserWithRole(@userIdd, @newName, @newAge ,@newPassword , @newRoleId);", parameters);
        //}

        public async Task AssignRoleToUser(int userId, int roleId)
        {
            await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"CALL AssignRoleToUser({userId}, {roleId})");
        }
        public async Task<List<UserwithRoleResponse>> GetUserById(int userId)
        {
            var result = await _context.UserWithRoleResponses
                   .FromSqlRaw($"CALL GetUserById({userId})")
                   .ToListAsync();
            return result;
        }
        public async Task<LoginResponse> Login(User model)
        {
            var emailParam = new MySqlParameter("@p_Email", model.Name);
            var passwordParam = new MySqlParameter("@p_Password", model.Password);
            var userObjectParam = new MySqlParameter("@p_UserObject", MySqlDbType.JSON)
            {
                Direction = ParameterDirection.Output
            };
            await _context.Database.ExecuteSqlRawAsync("CALL login(@p_Email, @p_Password, @p_UserObject)",
                emailParam, passwordParam, userObjectParam);

            if (userObjectParam.Value == DBNull.Value)
            {
                return null; // Return null for user not found
            }
            else
            {
                var userObjectJson = userObjectParam.Value.ToString();
                var user = JsonConvert.DeserializeObject<User>(userObjectJson);

                // Fetch role information using UserRoles
                var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.Userid == user.Userid);
                if (userRole == null)
                {
                    return null; // Return null if user role is not found
                }

                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == userRole.Roleid);
                if (role == null)
                {
                    return null; // Return null if role is not found
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Ei6z2c0Z1bwScDS0DFN3Sh6e1eEGJ8hM/VemTrLg6YI="));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var Claims = new[]
                {
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", role.RoleId.ToString()),
                    new Claim(ClaimTypes.Role, role.Role)

                };
                _logger.LogInformation("Claims added to token: {@Claims}", Claims);
                var token = new JwtSecurityToken("http://localhost:44380", "http://localhost:44380", Claims, expires: DateTime.Now.AddMinutes(5), signingCredentials: signingCredentials);
                //var tokenOptions = new JwtSecurityToken(
                //    issuer: "http://localhost:44380",
                //    audience: "http://localhost:44380",
                //    claims: new List<Claim>
                //    {       
                //        new Claim(ClaimTypes.Role, role.Role) ,
                //        new Claim(ClaimTypes.NameIdentifier, role.RoleId.ToString())// Include the user's role
                //    },
                //    expires: DateTime.Now.AddMinutes(5),
                //    signingCredentials: signingCredentials
                //    );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                var loginResponse = new LoginResponse
                {
                    User = user,
                    Role = role,
                    TokenString = tokenString
                };
                return loginResponse;
            }
        }


       
        public async Task<List<UserwithRoleResponse>> GetAllUsers()
        {
            var result = await _context.UserWithRoleResponses
                .FromSqlRaw("CALL GetAllUsers()")
                .ToListAsync();

            return result;
        }
    }
}
