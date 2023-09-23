using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Common.Schema.Models
{
    public class UserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Userroleid { get; set; }
        public int Userid { get; set; }
        public int Roleid { get; set; }
        [ForeignKey("userid")]
        public User User { get; set; }
        [ForeignKey("roleid")]
        public Roles Role { get; set; }
    }
}
