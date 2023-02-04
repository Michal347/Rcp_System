using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace RCP_Sys.ViewModels
{
    public class LoginViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region ICommands
        public ICommand LoginCommand { get; private set; }
        public ICommand RegisterCommand { get; private set; }
        private IUserService UserAdd;
        #endregion

        public event EventHandler<UserModel> LoginCompleted;

        public event EventHandler<string> RegisterRequested;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(x => LoginUser(),CanExecuteloginCommand);
            RegisterCommand = new RelayCommand(x => Register());
            UserAdd = new UserService();
        }

       

        #region ICommand handlers
        private void LoginUser()
        {
            ClearPropertyErrors(this, "UserLogin");
            UserModel output;
            using (var context = new RcpDbContext())
            {
                var found = context.Users.FirstOrDefault(x => x.Username == UserLogin);
               
                if (found != null)
                {
                    var valid = PasswordHasher.ComparePasswords(found.Password, Password);
                    if (!valid)
                    {
                        OnErrorCreated("UserLogin", "*Invalid password or username");
                        return;
                        
                    }
                    output = new UserModel() { Name = found.Name, Surname = found.Surname, Id = found.Id, DateTimeJoined= found.DateTimeJoined, Email=found.Email, Gender=found.Gender};
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserLogin), null);
                }
                else
                {
                    OnErrorCreated("UserLogin", "*Invalid password or username");
                    return;
                }
            }

            LoginCompleted.Invoke(this, output);
        }

        private void Register()
        {
            RegisterRequested.Invoke(this, UserLogin);
        }

            private bool CanExecuteloginCommand(object obj)
        {
            
                bool valiData;
                if (string.IsNullOrWhiteSpace(userLogin) || UserLogin.Length < 1 ||
                    Password == null || Password.Length < 1)
                    valiData = false;
                else
                    valiData = true;
                return valiData;
            
        }

        #endregion


        #region Event rising fields

        private string userLogin;

        public string UserLogin
        {
            get { return userLogin; }
            set
            {
                userLogin = value;
                OnPropertyChanged("UserLogin");
                ClearPropertyErrors(this, "UserLogin");      
            }
        }

            private string password;

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
                ClearPropertyErrors(this, "Password");
            }
        }


        #endregion

    }
}