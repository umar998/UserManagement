using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using UserManagement.Common.Schema.Models;
using UserManagement.Data.Services;
using UserManagement.Common.Filters;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RoleService _roleService;
    
        public RoleController(IConfiguration configuration, RoleService roleService)
        {
            _configuration = configuration;
            _roleService = roleService;
        }

        [HttpPost]
        [Route("AddRole")]
        [TypeFilter(typeof(Authorizations))]
        public Task<ActionResult<Roles>> AddRole(Roles r)
        {
            try
            {
                _ = _roleService.AddRole(r);
                return Task.FromResult<ActionResult<Roles>>(Ok(new { Message = "Role added." }));
            }
            catch (Exception ex)
            {
                return Task.FromResult<ActionResult<Roles>>(BadRequest(ex));
            }
        }

        [HttpGet]
        [Route("GetAllRoles")]
        [TypeFilter(typeof(Authorizations))]
        public async Task<ActionResult> GetAllRoles()
        {
            try
            {
                var result = await _roleService.GetAllRoles();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
