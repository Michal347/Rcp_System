using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Repository
{
    public class TimerService : ITimerService
    {
        public void Create(TimerModel time)
        {
            using (var context = new RcpDbContext())
            {
                context.Times.Add(time);
                context.SaveChanges();
            }
        }
    }
}
