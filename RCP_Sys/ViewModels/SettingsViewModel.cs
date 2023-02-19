using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RCP_Sys.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private IUserService getUsername;
        public ICommand ModifyText { get; private set; }
        public ICommand ModifyName { get; private set; }
        public ICommand ModifySurname { get; private set; }
        public ICommand ModifyEmail { get; private set; }
        public ICommand UpdateName { get; private set; }
        public ICommand UpdateSurname { get; private set; }
        public ICommand UpdateEmail { get; private set; }
        public ICommand UpdateUsername { get; private set; }

        public ICommand SaveName { get; private set; }
        public ICommand SaveSurname { get; private set; }
        public ICommand SaveEmail { get; private set; }
        public ICommand SaveUsername { get; private set; }


        private UserAccountInformation _UserInformation;
        public UserAccountInformation UserInformation
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
            UserInformation = new UserAccountInformation();
            getUsername = new UserService();

            LoadImage();
            CurrentUserLogged();
            UpdateName = new RelayCommand(x => UpdateDataName());
            UpdateSurname = new RelayCommand(x => UpdateDataSurname());
            UpdateEmail = new RelayCommand(x => UpdateDataEmail());
            UpdateUsername = new RelayCommand(x => UpdateDataUsername());
            ModifyText = new RelayCommand(x => ModifyUsername());
            ModifyEmail = new RelayCommand(x => ModEmail());
            ModifyName = new RelayCommand(x => ModName());
            ModifySurname = new RelayCommand(x => ModSurname());

            SaveName = new RelayCommand(x => SvName());
            SaveEmail = new RelayCommand(x => SvEmail());
            SaveUsername = new RelayCommand(x => SvUsername());
            SaveSurname = new RelayCommand(x => SvSurname());


            IsVisibleUsername = false;
            IsVisibleSurname = false;
            IsVisibleEmail = false;
            IsVisibleName = false;

           
        }

        private void SvSurname()
        {

            ErrorMessageSurname = "";
            IsVisibleSurname = false;
            IsVisibleModifySurname = true;
        }

        private void SvUsername()
        {
          
            IsVisibleUsername = false;
            IsVisibleModifyUsername = true;
            ErrorMessageUsername = "";
        }

        private void SvEmail()
        {

            IsVisibleEmail = false;
            IsVisibleModifyEmail = true;
            ErrorMessageEmail = "";
        }

        private void SvName()
        {
   
            IsVisibleName = false;
            IsVisibleModifyName = true;
            ErrorMessageName= "";
        }

        private void ModSurname()
        {
            IsVisibleSurname = true;
            IsVisibleModifySurname = false;
        }

        private void ModName()
        {
            IsVisibleName = true;
            IsVisibleModifyName = false;
        }

        private void ModEmail()
        {
            IsVisibleEmail = true;
            IsVisibleModifyEmail = false;
        }

        private void ModifyUsername()
        {
            IsVisibleUsername = true;
            IsVisibleModifyUsername = false;
        }

        private void UpdateDataName()
        {
            using (var context = new RcpDbContext())
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                if (user != null)
                {
                    if (string.IsNullOrWhiteSpace(UserInformation.Name))
                    {
                        ErrorMessageName = "*Name can't be empty";
                        return;
                    }
                    ErrorMessageName = "";
                    user.Name = UserInformation.Name;
                    context.Users.Update(user);
                    context.SaveChanges();
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserInformation.Username), null);

                }
            }

            IsVisibleName = false;


        }

        private void UpdateDataSurname()
        {
            using (var context = new RcpDbContext())
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                if (user != null)
                {
                    if (string.IsNullOrWhiteSpace(UserInformation.Name))
                    {
                        ErrorMessageSurname = "*Surname can't be empty";
                        return;
                    }
                    ErrorMessageSurname = "";
                    user.Surname = UserInformation.Surname;

                    context.Users.Update(user);
                    context.SaveChanges();
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserInformation.Username), null);

                }
            }

            IsVisibleSurname = false;


        }
        private void UpdateDataEmail()
        {
            using (var context = new RcpDbContext())
            {
                var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                var validEmail = EmailValidation.IsValidEmail(UserInformation.Email);
                if (string.IsNullOrWhiteSpace(UserInformation.Email))
                {
                    ErrorMessageEmail = "*Cannot be empty";
                    return;
                }

                else if (validEmail == false)
                {
                    ErrorMessageEmail = "*Invalid Email";
                    return;

                }

                user.Email = UserInformation.Email;
                context.Users.Update(user);
                context.SaveChanges();
                ErrorMessageEmail = "";
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserInformation.Username), null);

            }
        
            IsVisibleEmail = false;
            ErrorMessageEmail = "";
        }


        private void UpdateDataUsername()
        {
            ErrorMessageUsername = "";
            using (var context = new RcpDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == UserInformation.Id);

                if (string.IsNullOrWhiteSpace(UserInformation.Username))
                {
                    ErrorMessageUsername = "*Cannot be empty";
                    return;
                }

                else if (user != null && UserInformation.Username != Thread.CurrentPrincipal.Identity.Name)
                {
                    ErrorMessageUsername = "*Username already exist";
                    return;
                }
                    user.Username = UserInformation.Username;
                    context.Users.Update(user);
                    context.SaveChanges();
                ErrorMessageUsername = "";
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(UserInformation.Username), null);

                }
                IsVisibleUsername = false;

            }


        public void LoadImage()
        {

            using (var context = new RcpDbContext())
            {

                var q = (from s in context.Picture
                         where s.Username == Thread.CurrentPrincipal.Identity.Name
                         select s.ImageToByte).FirstOrDefault();

                if (q != null)
                {
                    Stream StreamObj = new MemoryStream(q);

                    BitmapImage BitObj = new BitmapImage();

                    BitObj.BeginInit();

                    BitObj.StreamSource = StreamObj;

                    BitObj.EndInit();

                    this.ImageSource = BitObj;
                }
                else
                {
                    var gender = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                    if (gender != null)
                    {
                        var value = "Female";
                        Boolean result = gender.Gender.Contains(value);
                        if (result == true)
                        {
                            string imagePath = "\\Images\\woman.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }
                        if (result == false)
                        {
                            string imagePath = "\\Images\\man.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }

                        if (result == false && gender.IsUserAdmin == true)
                        {
                            string imagePath = "\\Images\\businessman.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }
                    }
                }
            }

        }



        private void CurrentUserLogged()
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                {
                    UserInformation.Id = user.Id;
                    UserInformation.Username = user.Username;
                    UserInformation.Name = user.Name;
                    UserInformation.Surname = user.Surname;
                    UserInformation.DateJoin = user.DateTimeJoined.ToString("MM/dd/yyyy");
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

            private bool _IsVisibleModifySurname = true;
        public bool IsVisibleModifySurname
        {
            get => _IsVisibleModifySurname;
            set
            {
                _IsVisibleModifySurname = value;
                OnPropertyChanged(nameof(IsVisibleModifySurname));
            }

        }

            private bool _IsVisibleModifyEmail = true;
        public bool IsVisibleModifyEmail
        {
            get => _IsVisibleModifyEmail;
            set
            {
                _IsVisibleModifyEmail = value;
                OnPropertyChanged(nameof(IsVisibleModifyEmail));
            }

        }
            private bool _IsVisibleModifyUsername = true;
        public bool IsVisibleModifyUsername
        {
            get => _IsVisibleModifyUsername;
            set
            {
                _IsVisibleModifyUsername = value;
                OnPropertyChanged(nameof(IsVisibleModifyUsername));
            }

        }

        private string _ErrorMessageUsername;
        public string ErrorMessageUsername
        {
            get => _ErrorMessageUsername;
            set
            {
                _ErrorMessageUsername = value;
                OnPropertyChanged(nameof(ErrorMessageUsername));
            }

        }

        private string _ErrorMessageEmail;
        public string ErrorMessageEmail
        {
            get => _ErrorMessageEmail;
            set
            {
                _ErrorMessageEmail = value;
                OnPropertyChanged(nameof(ErrorMessageEmail));
            }

        }

        private string _ErrorMessageName;
        public string ErrorMessageName
        {
            get => _ErrorMessageName;
            set
            {
                _ErrorMessageName = value;
                OnPropertyChanged(nameof(ErrorMessageName));
            }

        }
        private string _ErrorMessageSurname;
        public string ErrorMessageSurname
        {
            get => _ErrorMessageSurname;
            set
            {
                _ErrorMessageSurname = value;
                OnPropertyChanged(nameof(ErrorMessageSurname));
            }

        }

        private ImageSource _ImageSource;
        public ImageSource ImageSource
        {
            get { return this._ImageSource; }
            set { this._ImageSource = value; this.OnPropertyChanged("ImageSource"); }
        }
    }
}
