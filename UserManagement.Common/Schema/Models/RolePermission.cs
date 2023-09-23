using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace UserManagement.Common.Schema.Models
{
    public class RolePermission
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Rolepermissionid { get; set; }
        public int Permissionid { get; set; } 
        public int Roleid { get; set; } 
        public bool Isallowed { get; set; }
        public Roles Role { get; set; }
        public Permission Permission { get; set; }
    }
}
