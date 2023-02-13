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
using System.Xml.Serialization;
using System.Collections;
using System.Windows.Data;
using System.ComponentModel;
using System.Reflection;
using RCP_Sys.Utilities;
using RCP_Sys.DAL.Interface;
using RCP_Sys.Services;

namespace RCP_Sys.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public IUserService RemoveUser;
        public IDialogService ChangeUser;
        public IUserService GetUser;
        public ICollectionView UserIcollection { get; set; }
        public UserViewModel()
         {
            LoadUsers();
            UserIcollection = CollectionViewSource.GetDefaultView(UserCollection);
            DeleteCommand = new RelayCommand(DeleteUser);
            EditCommand = new RelayCommand(EditUser);
            RemoveUser = new UserService();
            GetUser = new UserService();
            ChangeUser= new DialogService();
            UserSum();
            GroupFilter gf = new GroupFilter();
            gf.AddFilter(UsernameFilter);
            UserIcollection.Filter = gf.Filter;
            UserIcollection.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));
        }

        private void EditUser(object obj)
        {
            ChangeUser.ShowDialog();
        }

        private bool UsernameFilter(object obj)
        {
            if (obj is UserModel user)
            {
                return user.Username.Contains(Usernamefiltr);
            }
            return false;
        }

        private void UserSum()
        {
            AllMembersCount = GetUser.GetUsers().Count;
        }

        private void DeleteUser(object obj)
        {
            using (var context = new RcpDbContext())
            {
                var emp = obj as UserModel;
                            if (emp.IsUserAdmin == false)
                            {
                                context.Users.Remove(emp);
                                UserCollection.Remove(emp);
                                UserIcollection.Refresh();
                                context.SaveChanges();
                                UserSum();
                                ClearPropertyErrors(this, "ErrorMessage");
                            }
                                
                            else
                            {
                                OnErrorCreated("ErrorMessage", "*User can't be Admin");
                            }
            }
            
        }  

        public void LoadUsers()
        {
            using (var context = new RcpDbContext())
            {
                var q = from a in context.Users
                        where a.Username != null 
                        select a;
                UserCollection = new ObservableCollection<UserModel>(q);

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
                ClearPropertyErrors(this, "ErrorMessage");
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

            private int _AllMembersCount;
        public int AllMembersCount
        {
            get
            {
                return _AllMembersCount;
            }
            set
            {
                _AllMembersCount = value;
                OnPropertyChanged(nameof(AllMembersCount));
                
            }
        }

         private string _Usernamefiltr =string.Empty;
        public string Usernamefiltr
        {
            get
            {
                return _Usernamefiltr;
            }
            set
            {
                _Usernamefiltr = value;
                OnPropertyChanged(nameof(Usernamefiltr));
                UserIcollection.Refresh();
            }
        }

    }
}
