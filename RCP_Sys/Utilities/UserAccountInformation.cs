using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Utilities
{
    public class UserAccountInformation
    {
        [Key]

        public int Id { get; set; }
        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string DisplayName { get; set; }

        public string DateJoin { get; set; }

        public string Email { get; set; }

        public bool IsUserAdmin { get; set; }

        public string Gender { get; set; }
    }
}
