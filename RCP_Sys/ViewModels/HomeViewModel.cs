
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RCP_Sys.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private UserAccountInformation _UserInformation;
        private IUserService getUsername;
        private IUserService getGender;
        public ICommand OpenFile { get; set; }

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

            public HomeViewModel()
            {
                getUsername = new UserService();
                getGender = new UserService();
                UserInformation = new UserAccountInformation();
                CurrentUserInformation();


            var gender = getGender.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (gender != null)
            {
                var value = "Female";
                Boolean result = gender.Gender.Contains(value);
                if (result == true)
                {
                    string imagePath = "\\Images\\woman.png";
                    this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                }
                if(result == false)
                {
                    string imagePath = "\\Images\\man.png";
                    this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                }

               if(result == false && gender.IsUserAdmin == true)
                {
                    string imagePath = "\\Images\\businessman.png";
                    this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                }
            }
        }
        public void CurrentUserInformation()
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                {
                    UserInformation.Username = user.Username;
                    UserInformation.Name = user.Name;
                    UserInformation.Surname = user.Surname;
                    UserInformation.DateJoin = user.DateTimeJoined.ToString("MM/dd/yyyy");
                    UserInformation.Email = user.Email;
                    
                    
                };
                

            }
            else
            {
                UserInformation.Username = "Invalid, Information";
                UserInformation.Name = "Invalid, Information";
                UserInformation.Surname = "Invalid, Information";
                UserInformation.Email = "Invalid, Information";
                UserInformation.DateJoin = "Invalid, Information";

            }
        }
            private BitmapImage _ImageSource;
        public BitmapImage ImageSource
        {
            get { return this._ImageSource; }
            set { this._ImageSource = value; this.OnPropertyChanged("ImageSource"); }
        }

    }

}
