using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RCP_Sys.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private UserAccountModel _UserInformation;
        private IUserService getUsername; 

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

            public HomeViewModel()
            {
            getUsername = new UserService();
            UserInformation = new UserAccountModel();
                CurrentUserInformation();
            }
        private void CurrentUserInformation()
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
            else
            {
                UserInformation.Username = "Invalid, Information";
                UserInformation.Name = "Invalid, Information";
                UserInformation.Surname = "Invalid, Information";
               

            }
        }

            private ImageSource _yourImage;
        public ImageSource YourImage
        {
            get { return _yourImage; }
            set
            {
                _yourImage = value;
                OnPropertyChanged(nameof(YourImage));
            }
        }
    }
    
}
