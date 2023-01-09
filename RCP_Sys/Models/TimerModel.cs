using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Models
{
    public class TimerModel
    {
        [Key]
        public int TimerId { get; set; }
        public string Username { get; set; }
        public DateTime StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public DateTime DateCreate { get; set; }
        public TimeSpan StartTimerValue{ get; set; }

        public TimeSpan EndTimerValue { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }

    }
}
