using RCP_Sys.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using RCP_Sys.Db;
using System.Collections.ObjectModel;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using System.Windows.Forms;
using System.IO.Packaging;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Security.Cryptography;

namespace RCP_Sys.Repository
{
    public class UserService: IUserService
    {
        private RcpDbContext context;

        public UserService(RcpDbContext context)
        {
            this.context = context;
        }

        public UserService()
        {
        }

        public void Create(UserModel user)
        {           
               context.Users.Add(user);
               context.SaveChanges();    
        }

        public UserModel GetUserModels(string username)
        {
                return context.Users.FirstOrDefault(x => x.Username == username);      
        }

        public void Save()
        {
            
                context.SaveChanges();
            
        }

        public void Remove(int id)
        {

                UserModel user = context.Users.Find(id);
                context.Users.Remove(user);
                context.SaveChanges();
            
        }

        public List<UserModel> GetStudents()
        {
            return context.Users.ToList();
        }

    }
}
