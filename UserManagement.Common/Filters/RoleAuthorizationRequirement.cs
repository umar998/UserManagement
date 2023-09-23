using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Common.Filters
{
    public class RoleAuthorizationRequirement : IAuthorizationRequirement
    {
        public string RequiredRole { get; }

        public RoleAuthorizationRequirement(string requiredRole)
        {
            RequiredRole = requiredRole;
        }
    }
}
