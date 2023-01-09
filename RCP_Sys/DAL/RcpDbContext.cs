
using RCP_Sys.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RCP_Sys.Db
{
    public class RcpDbContext : DbContext
    {
        public RcpDbContext()
        : base("name=RcpSysDb")
        {
            Database.SetInitializer(new RCPInit());
        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TimerModel> Times { get; set; }

    }
}

