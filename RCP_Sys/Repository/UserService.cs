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

namespace RCP_Sys.Repository
{
    public class UserService: IUserService
    {

        public void Create(UserModel user)
        {
            using (var context = new RcpDbContext())
            {
               context.Users.Add(user);
               context.SaveChanges();
            }
        }

        public UserModel GetUserModels(string username)
        {
            using (var context = new RcpDbContext())
            {

                return context.Users.FirstOrDefault(x => x.Username == username);
            }
        }

        public void Save()
        {
            using (var context = new RcpDbContext())
            {
                context.SaveChanges();
            }
        }

        public void Remove(int id)
        {
            using (var context = new RcpDbContext())
            {
                UserModel user = context.Users.Find(id);
                context.Users.Remove(user);
                context.SaveChanges();
            }
        }
    }
}
