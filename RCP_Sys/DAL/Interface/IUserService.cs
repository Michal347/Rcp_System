using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RCP_Sys.Models
{
    public interface IUserService
    {
        UserModel GetUserModels(string Username);
        void Create(UserModel user);
        void Save();
        void Remove(int id);
        
    }
}
