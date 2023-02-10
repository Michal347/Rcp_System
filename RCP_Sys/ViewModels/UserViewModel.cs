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

namespace RCP_Sys.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        
        public ICommand DeleteCommand { get; }
        public IUserService RemoveUser;
        public IUserService GetUser;
        public ICollectionView UserIcollection { get; set; }

        public UserViewModel()
         {
            UserIcollection = CollectionViewSource.GetDefaultView(UserCollection);
            DeleteCommand = new RelayCommand(DeleteUser);
            RemoveUser = new UserService();
            GetUser = new UserService();
            LoadUsers();
            UserSum();
            GroupFilter gf = new GroupFilter();
            gf.AddFilter(UsernameFilter);
            UserIcollection.Filter = gf.Filter;
        }


        public class GroupFilter
        {
            private List<Predicate<object>> _filters;

            public Predicate<object> Filter { get; private set; }

            public GroupFilter()
            {
                _filters = new List<Predicate<object>>();
                Filter = InternalFilter;
            }

            private bool InternalFilter(object o)
            {
                foreach (var filter in _filters)
                {
                    if (!filter(o))
                    {
                        return false;
                    }
                }

                return true;
            }

            public void AddFilter(Predicate<object> filter)
            {
                _filters.Add(filter);
            }

            public void RemoveFilter(Predicate<object> filter)
            {
                if (_filters.Contains(filter))
                {
                    _filters.Remove(filter);

                }
            }
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
                                context.SaveChanges();
                                UserSum();
                            }
                        }                
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

            }
        }


    }
}
