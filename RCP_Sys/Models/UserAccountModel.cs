using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Models
{
    public class UserAccountModel
    {
        [Key]
        public string Username {get; set; }

        public string Name {get; set; }

        public string Surname {get; set; } 

        public string DisplayName { get; set; }

        public DateTime DateJoin { get; set; }

        public string Email { get; set; }

        public bool IsUserAdmin { get; set; }

    }
}
