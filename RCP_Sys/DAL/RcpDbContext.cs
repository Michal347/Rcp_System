
using Microsoft.EntityFrameworkCore;
using RCP_Sys.Models;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RCP_Sys.Db
{
    public class RcpDbContext : DbContext
    {
       
        public DbSet<UserModel> Users { get; set; }
        public DbSet<ProjectModel> Projects { get; set; }
        public DbSet<TimerModel> Times { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hash = PasswordHasher.HashPassword("Admin");
            modelBuilder.Entity<UserModel>().HasData(
                new UserModel
                {
                    Id = 1,
                    Username = "Admin",
                    Password = hash,
                    Surname = "Admin",
                    Name = "Admin",
                    DateTimeJoined = DateTime.Now,
                    Email = "admin@gmail.com",
                    IsUserAdmin = true,

                });
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;Database=RcpSystemDb;Integrated Security=True;");
            base.OnConfiguring(optionsBuilder);
        }

    }
}

