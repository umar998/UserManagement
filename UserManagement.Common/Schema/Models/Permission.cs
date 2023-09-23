using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace UserManagement.Common.Schema.Models
{
    public class Permission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permissionid { get; set; }
        public string Actionname { get; set; }
        public string Controllername { get; set; }
        public ICollection<RolePermission> RolesPermissions { get; set; }
    }
}
