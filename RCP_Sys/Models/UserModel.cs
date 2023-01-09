using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; } 

        public DateTime DateTimeJoined { get; set; }

        public string Email  { get; set; }

        public bool IsUserAdmin { get; set; }
    }
}
