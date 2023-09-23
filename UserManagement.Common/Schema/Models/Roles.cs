using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace UserManagement.Common.Schema.Models
{
    public class Roles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int RoleId { get; set; }
        public string Role { get; set; }
        public ICollection<RolePermission> RolesPermissions { get; set; }
    }
}
