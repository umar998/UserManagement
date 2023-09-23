using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using UserManagement.Common.Schema.Models;
using UserManagement.Data.Services;
using UserManagement.Common.Filters;
using Microsoft.AspNetCore.Hosting;

namespace UserManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
     
        private readonly PermissionService _permissionService;
        
        public PermissionsController(IConfiguration configuration,PermissionService permissionService)
        {
            _configuration = configuration;
            _permissionService= permissionService;
        }

        [HttpPost]
        [Route("AddPermissions")]
        [TypeFilter(typeof(Authorizations))]
        public ActionResult<Permission> AddPermissions(Permission r)
        {
            try
            {
                _=_permissionService.AddPermission(r);
                return Ok(new {Message = "Permissions added."});
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("AddRolesPermissions")]
        [TypeFilter(typeof(Authorizations))]
        public ActionResult<RolePermission> AddRolesPermissions(RolePermission r)
        {
            try
            {
                _ = _permissionService.AddRolesPermission(r);
                return Ok(new { Message = "Permissions added." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetAllPermissions")]
        [TypeFilter(typeof(Authorizations))]
        public async Task<ActionResult> GetAllPermissions()
        {
            try
            {
                var result = await _permissionService.GetAllPermissions();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        [Route("UpdatePermission")]
        [TypeFilter(typeof(Authorizations))]
        public ActionResult<RolePermission> UpdatePermission([FromBody] PermissionUpdateRequest request)
        {
            try
            {
                _ = _permissionService.UpdatePermisssion(request);
                return Ok(new { Message = "Permissions added." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
