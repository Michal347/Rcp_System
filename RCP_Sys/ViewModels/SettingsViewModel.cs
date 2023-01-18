using Microsoft.EntityFrameworkCore;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace RCP_Sys.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IUserService getUsername;
        public ICommand ModifyText { get; private set; }
        public ICommand Update { get; private set; }


         private UserAccountModel _UserInformation;
        public UserAccountModel UserInformation
        {
            get
            {
                return _UserInformation;
            }
            set
            {
                _UserInformation = value;
                OnPropertyChanged(nameof(UserInformation));
            }
        }
        public SettingsViewModel()
        {
            UserInformation = new UserAccountModel();
            getUsername = new UserService();
           
            CurrentUserLogged();
            Update = new RelayCommand(x=>UpdateData());
            ModifyText = new RelayCommand(x => Modify());
            IsVisibleUsername = false;
        }

        private void Modify()
        {
            IsVisibleUsername = true;
        }

        private void UpdateData()
        {
            using (var context = new RcpDbContext())
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                if (user != null)
                {
                    user.Username = UserInformation.Username;
                    user.Name = UserInformation.Name;
                    user.Surname = UserInformation.Surname;
                    user.Email = UserInformation.Email;

                    context.Users.Update(user);
                    context.SaveChanges();
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserInformation.Username), null);
                    
                }
            }
        }

        private void CurrentUserLogged()
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                {
                    UserInformation.Username = user.Username;
                    UserInformation.Name = user.Name;
                    UserInformation.Surname = user.Surname;
                    UserInformation.DateJoin = user.DateTimeJoined;
                    UserInformation.Email = user.Email;

                };


            }     
        }

            private bool _IsVisibleUsername = true;
        public bool IsVisibleUsername
        {
            get => _IsVisibleUsername;
            set
            {
                _IsVisibleUsername = value;
                OnPropertyChanged(nameof(IsVisibleUsername));
            }

        }

    }
}
