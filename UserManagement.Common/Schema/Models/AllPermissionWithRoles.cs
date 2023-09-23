using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Common.Schema.Models
{
    public class AllPermissionWithRoles
    {


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permissionid { get; set; }
        public string Actionname { get; set; }
        public string Controllername { get; set; }

        public int? Rolepermissionid { get; set; }

        public int? Roleid { get; set; }
        public int? Isallowed { get; set; }
    }
}
