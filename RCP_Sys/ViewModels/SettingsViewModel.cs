using Microsoft.EntityFrameworkCore;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using System;
using System.Collections.Generic;
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
        private IUserService Modify;
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
            Modify = new UserService();
            CurrentUserData();
            Update = new RelayCommand(x=>UpdateData());
        }

    
        private void UpdateData()
        {
            using (var context = new RcpDbContext())
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                if (user != null)
                {
                    user.Username = Login;
                    user.Name = Name;
                    user.Surname = surname;
                    user.Email = Email;

                    context.Users.Update(user);
                    context.SaveChanges();
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Login), null); ;
                }
            }
        }


        private void CurrentUserData()
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

            private string login;

        public string Login
        {

            get
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                login=user.Username;
                return login;
            }
            set
            {
                login = value;
                OnPropertyChanged("Login");
                ClearPropertyErrors(this, "Login");
            }
        }

            private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

            private string surname;

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

            private string email;

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
                ClearPropertyErrors(this, "Email");
            }
        }
    }
}
