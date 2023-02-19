using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Models
{
    public class ProfilePictureModel
    {
        [Key]
        public int Id { get; set; } 
        
        public string ImagePath { get; set; }
       
        public byte[] ImageToByte { get; set; }
        
        public string Username { get; set; }
    }
}
