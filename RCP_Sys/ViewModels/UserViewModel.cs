using Microsoft.Win32;
using RCP_Sys.Db;
using RCP_Sys.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Xml.Linq;
using RCP_Sys.Repository;

namespace RCP_Sys.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public ICommand UserDelete { get; }
        public ICommand Refresh { get; }
        public IUserService RemoveUser;

        public UserViewModel()
         {
            UserDelete = new RelayCommand(x => Delete());
            Refresh = new RelayCommand(x => RefreshUser());
            RemoveUser = new UserService();
            UserListView();
            LoadUsers();
         }

        private void Delete()
        {
            
            using (var context = new RcpDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Id == SelectedComboItem);


                if (user != null)
                {
                    if (user.IsUserAdmin == false)
                    {
                        RemoveUser.Remove(user.Id);
                        RefreshUser();
                    }
                    else
                    {
                        ErrorMessage = "*User cannot be admin";

                    }
                }

            }
            new UserViewModel();
        }

        public void LoadUsers()
        {
            using (var context = new RcpDbContext())
            {
                var q = from s in context.Users
                        select s;

                UserCollection = new ObservableCollection<UserModel>(q);

            }
        }


        private void RefreshUser()
        {
            UserListView();
        }

        public void UserListView()
        {
            using (var context = new RcpDbContext())
            {
                var User = from a in context.Users
                            select a;

                UserCollection = new ObservableCollection<UserModel>(User);

            }
        }
            private ObservableCollection<UserModel> _UserCollection;

        public ObservableCollection<UserModel> UserCollection
        {
            get { return _UserCollection; }
            set { _UserCollection = value; OnPropertyChanged("UserCollection"); }
        }

         private int _ExistUser;

        public int ExistUser
        {
            get { return _ExistUser; }
            set
            {
                _ExistUser = value;
                OnPropertyChanged("ExistUser");  
            }
        }

         private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private int _selectedComboItem;
        public int SelectedComboItem
        {
            get
            {
                return _selectedComboItem;
            }
            set
            {
                _selectedComboItem = value;
                OnPropertyChanged(nameof(SelectedComboItem));
                ClearPropertyErrors(this, "SelectedComboItem");
            }
        }

        
    }
}
