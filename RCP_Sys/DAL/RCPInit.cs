using RCP_Sys.Models;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Db
{
    public class RCPInit : System.Data.Entity.CreateDatabaseIfNotExists<RcpDbContext>
    {

        protected override void Seed(RcpDbContext context)
        {
            var hash = PasswordHasher.HashPassword("Admin");
            var User = new List<UserModel>
            {
                new UserModel()
                {
                    Name = "Admin",
                    Password=hash,
                    Surname="Admin",
                    Username="Admin",
                    DateTimeJoined=DateTime.Now,
                    Email="admin@gmail.com",
                    IsUserAdmin=true,
                    }
                };
            User.ForEach(s => context.Users.Add(s));
            context.SaveChanges();

        }
    }

}   
