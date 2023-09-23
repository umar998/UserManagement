using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using UserManagement.Common.Filters;
using UserManagement.Common.Schema.Models;
using UserManagement.Data.Services;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;

        public UsersController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;

        }
        [HttpPost]
        [Route("LoginSP")]
        public async Task<ActionResult<User>> LoginSP([FromBody] User model)
        {
            try
            {
                var result = await _userService.Login(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]
        [Route("AddUser")]
        //[TypeFilter(typeof(Authorizations))]
        public ActionResult<User> AddUser(User r)
        {
            try
            {
                _ = _userService.AddUser(r);
                return Ok(new { Message = "Data added." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        [HttpPut("UpdateUserWithRoles")]
        [TypeFilter(typeof(Authorizations))]
        public ActionResult<UpdateUserWithRole> UpdateUserWithRoles(UpdateUserWithRole r)
        {
            try
            {
                _ = _userService.UpdateUserWithRole(r);
                return Ok(new { Message = "Data updated." });
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = 400,
                    Errors = new
                    {
                        Message = ex.Message, // Include the exception message
                                              // You can include additional error details if needed
                    }
                };

                return BadRequest(errorResponse);
            }
        }




        [HttpPost]
        [Route("AssignRoleToUser")]
        [TypeFilter(typeof(Authorizations))]
        public ActionResult<UserRoles> AssignRoleToUser(int userId, int roleId)
        {
            try
            {
                _ = _userService.AssignRoleToUser(userId, roleId);
                return Ok(new { Message = "Data added." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [Route("GetUserById")]
        [TypeFilter(typeof(Authorizations))]
        public async Task<ActionResult> GetUserById(int userId)
        {
            try
            {
                var result = await _userService.GetUserById(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete]
        [Route("DeleteUserAndRoles")]
        [TypeFilter(typeof(Authorizations))]
        public async Task<ActionResult> DeleteUserAndRoles(int userId)
        {
            try
            {
                // Call the DeleteUserAndRoles method to delete the user and roles
                await _userService.DeleteUserAndRoles(userId);

                // Optionally, return a success message or status code
                return Ok(new { Message = "User and associated roles deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        [TypeFilter(typeof(Authorizations))]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
