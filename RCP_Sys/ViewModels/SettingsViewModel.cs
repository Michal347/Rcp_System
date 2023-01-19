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
        public ICommand ModifyName { get; private set; }
        public ICommand ModifySurname { get; private set; }
        public ICommand ModifyEmail { get; private set; }
        public ICommand Update { get; private set; }
        
        public ICommand SaveName { get; private set; }


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
            ModifyText = new RelayCommand(x => ModifyUsername());
            ModifyEmail = new RelayCommand(x => ModEmail());
            ModifyName = new RelayCommand(x => ModName());
            ModifySurname = new RelayCommand(x => ModSurname());

            SaveName= new RelayCommand(x => SvName());

            IsVisibleUsername = false;
            IsVisibleSurname= false;
            IsVisibleEmail = false;
            IsVisibleName = false;
        }

        private void SvName()
        {
            if(string.IsNullOrWhiteSpace(UserInformation.Name))
            {
                OnErrorCreated("UserInformation.Name", "Cannot be empty");
                return;
            }

            IsVisibleName = false;
            IsVisibleModifyName = true;
            IsVisibleSaveChange = true;
        }

        private void ModSurname()
        {
           IsVisibleSurname = true;
            IsVisibleSaveChange = false;
        }

        private void ModName()
        {
            IsVisibleName=true;
            IsVisibleModifyName = false;
            IsVisibleSaveChange = false;
        }

        private void ModEmail()
        {
            IsVisibleEmail=true;
            IsVisibleSaveChange = false;
        }

        private void ModifyUsername()
        {
            IsVisibleUsername = true;
            IsVisibleSaveChange = false;
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


        private bool _IsVisibleName = true;
        public bool IsVisibleName
        {
            get => _IsVisibleName;
            set
            {
                _IsVisibleName = value;
                OnPropertyChanged(nameof(IsVisibleName));
            }

        }

        private bool _IsVisibleSurname = true;
        public bool IsVisibleSurname
        {
            get => _IsVisibleSurname;
            set
            {
                _IsVisibleSurname = value;
                OnPropertyChanged(nameof(IsVisibleSurname));
            }

        }

        private bool _IsVisibleEmail = true;
        public bool IsVisibleEmail
        {
            get => _IsVisibleEmail;
            set
            {
                _IsVisibleEmail = value;
                OnPropertyChanged(nameof(IsVisibleEmail));
            }

        }

        private bool _IsVisibleModifyName = true;
        public bool IsVisibleModifyName
        {
            get => _IsVisibleModifyName;
            set
            {
                _IsVisibleModifyName = value;
                OnPropertyChanged(nameof(IsVisibleModifyName));
            }

        }

        private bool _IsVisibleSaveChange = true;
        public bool IsVisibleSaveChange
        {
            get => _IsVisibleSaveChange;
            set
            {
                _IsVisibleSaveChange = value;
                OnPropertyChanged(nameof(IsVisibleSaveChange));
            }

        }

    }
}
