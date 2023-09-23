using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Common.Schema.Models
{
    public class PermissionUpdateRequest
    {
        public int Rolepermissionid { get; set; }
        public int Isallowed { get; set; }
    }
}
