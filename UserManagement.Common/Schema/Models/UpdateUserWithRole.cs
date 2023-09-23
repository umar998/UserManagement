using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Common.Schema.Models
{
    public class UpdateUserWithRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Userid { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public string? Password { get; set; }
        public int? Roleid { get; set; }
    }
}
