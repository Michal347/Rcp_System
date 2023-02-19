using RCP_Sys.Db;
using RCP_Sys.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using RCP_Sys.Repository;
using System.Windows.Data;
using System.ComponentModel;
using RCP_Sys.Utilities;
using RCP_Sys.DAL.Interface;
using RCP_Sys.Services;
using RCP_Sys.Views;
using System.Threading;
using System.Globalization;

namespace RCP_Sys.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        
        public ICommand DeleteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand GetGender { get; }
        public IUserService RemoveUser;
        public IDialogService ChangeUser;
        public IUserService GetUser;
        private ICollectionView _UserIcollection;
        public ICollectionView UserIcollection
        {
            get { return _UserIcollection; }
            set { _UserIcollection = value; OnPropertyChanged("UserIcollection"); }
        }

        public UserViewModel()
         {
            RefreshDatauser();
            UserIcollection = CollectionViewSource.GetDefaultView(UserCollection);
            DeleteCommand = new RelayCommand(DeleteUser);
            EditCommand = new RelayCommand(EditUser);
            UpdateCommand = new RelayCommand(x => UpdateData(), CanExecuteUpdate);
            GetGender = new RelayCommand(executemethod, canexecutemethod);
            RemoveUser = new UserService();
            GetUser = new UserService();
            ChangeUser= new DialogService();
            UserSum();
            GroupFilter gf = new GroupFilter();
            gf.AddFilter(UsernameFilter);
            UserIcollection.Filter = gf.Filter;
            UserIcollection.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));

        }

        private bool CanExecuteUpdate(object obj)
        {
            bool valiData;

            if ( 
                string.IsNullOrWhiteSpace(Username) || Username.Length < 1)

                valiData = false;
            else
                valiData = true;
            return valiData;
        }

        private bool canexecutemethod(object parameter)
        {
            if (parameter != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void executemethod(object parameter)
        {
            Gender = (string)parameter;
        }

        private void UpdateData()
        {
            ClearPropertyErrors(this, "Username");
            using (var context = new RcpDbContext())
            {
                ClearPropertyErrors(this, "Email");
                var validEmail = EmailValidation.IsValidEmail(Email);
                var found = context.Users.FirstOrDefault(x => x.Id == ID);
                var User = context.Users.FirstOrDefault(x => x.Username == Username);
                var q = (from s in context.Picture
                         where s.Username == found.Username
                         select s).FirstOrDefault();

                if (User != null)
                {
                    if (found.Username == Username)
                    {
                        if (validEmail == true)
                        {
                                
                                found.Name = Name;
                                found.Surname = surname;
                                found.Email = Email;
                                found.Gender = Gender;
                                found.IsUserAdmin = HybridSeed;
                                context.Users.Update(found);
                                context.SaveChanges();
                                RefreshDatauser();
                                ClearPropertyErrors(this, "Email");
                            
        
                        }
                        else
                        {
                            OnErrorCreated("Email", "*Invalid Email");
                            return;
                        }
                    }

                    else
                    {
                        OnErrorCreated("Username", "*Username already exist");
                        return;

                    }

                }   
                    q.Username= Username;
                    found.Username = Username;
                    found.Name = Name;
                    found.Surname = surname;
                    found.Email = Email;
                    found.Gender = Gender;
                    found.IsUserAdmin = HybridSeed;
                    context.Users.Update(found);
                    context.SaveChanges();
                    RefreshDatauser();
                    ClearPropertyErrors(this, "Email");
                    ClearPropertyErrors(this, "Username");

                
            }

        }
        private void RefreshDatauser()
        {
            LoadUsers();
            UserIcollection = CollectionViewSource.GetDefaultView(UserCollection);
            GroupFilter gf = new GroupFilter();
            gf.AddFilter(UsernameFilter);
            UserIcollection.Filter = gf.Filter;
            UserIcollection.SortDescriptions.Add(new SortDescription("Username", ListSortDirection.Ascending));
        }

        private void EditUser(object obj)
        {
            ClearPropertyErrors(this, "ErrorMessage");
            using (var context = new RcpDbContext())
            {

                var user = GetUser.GetUsers();
                var emp = obj as UserModel;
                if (emp != null)
                {
                    if (emp.Username != "Admin")
                    {
                        var dialog = new EditUserWindow()
                        {
                            DataContext = this,
                        };
                        dialog.Show();
                        surname = emp.Surname;
                        Username = emp.Username;
                        Gender = emp.Gender;
                        Email = emp.Email;
                        Name = emp.Name;
                        ID = emp.Id;
                        HybridSeed = emp.IsUserAdmin;
                        ClearPropertyErrors(this, "ErrorMessage");
                    }
                    else
                    {
                        OnErrorCreated("ErrorMessage", "*User can't be Admin");
                        return;
                    }
                }
                else
                {
                    OnErrorCreated("ErrorMessage", "Error");
                    return;
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
            ClearPropertyErrors(this, "ErrorMessage");
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
                ClearPropertyErrors(this, "ErrorMessage");
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
                UserIcollection.Refresh();
                OnPropertyChanged(nameof(Usernamefiltr));
               
            }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        private string _Surname;
        public string surname
        {
            get
            {
                return _Surname;
            }
            set
            {
                _Surname = value;
                OnPropertyChanged("surname");

            }
        }

        private string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                OnPropertyChanged("Name");

            }
        }

        private int _ID;
        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
                OnPropertyChanged(nameof(ID));
            }
        }
        private string _Username;
        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                _Username = value;
                OnPropertyChanged("Username");

            }
        }

        private string _Email;
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
                OnPropertyChanged("Email");
                ClearPropertyErrors(this, nameof(Email));

            }
        }

            private bool hybridSeed;

        public bool HybridSeed
        {
            get { return hybridSeed; }
            set
            {
                hybridSeed = value;
                OnPropertyChanged(nameof(HybridSeed));
            }
        }

        private string _gender = "Male";

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

    }
}
