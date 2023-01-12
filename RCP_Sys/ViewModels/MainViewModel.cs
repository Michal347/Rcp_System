using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RCP_Sys.ViewModels
{
    public class MainViewModel : BaseViewModel, INotifyPropertyChanged
    {

        #region ICommand

        public ICommand ShowHomeView { get; }
        public ICommand ShowTimerView { get; }
        public ICommand ShowProjectView { get; }
        public ICommand ShowTimesheetView { get; }
        public ICommand ShowUserView { get; }
        public ICommand ShowUserHistory { get; }
        public ICommand ShowSettingView { get; }

        #endregion

        #region View Fields

        private UserAccountModel userAccount;
        private IUserService GetUsername;
        private object _CurrentChildView;
        public TimerViewModel _timerViewModel;
        public HomeViewModel _homeViewModel;
        public ProjectViewModel _projectViewModel;
        public TimeSheetViewModel _timeSheetViewModel;
        public UserViewModel _userViewModel;
        public UserHistoryViewModel _userHistoryViewModel;
        public SettingsViewModel _settingsViewModel;
        #endregion
        public MainViewModel()
        {
            GetUsername = new UserService();
            var user = GetUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                if (user.IsUserAdmin == false)
                {
                    _btnUpdateVisibility = true;
                }
                else
                {
                    _btnUpdateVisibility = false;
                }
            }

            if (user != null)
            {
                if (user.IsUserAdmin == false)
                {
                    _btnAdminVisi = false;
                }
                else
                {
                    _btnAdminVisi = true;
                }
            }


            _timerViewModel = new TimerViewModel();
            _homeViewModel = new HomeViewModel();
            _projectViewModel = new ProjectViewModel();
            _timeSheetViewModel = new TimeSheetViewModel();
            _userViewModel = new UserViewModel();
            _userHistoryViewModel = new UserHistoryViewModel();
            _settingsViewModel = new SettingsViewModel();
            GetUsername = new UserService();
            UserAccount = new UserAccountModel();
            CurrentUserData();


            ShowHomeView = new RelayCommand(Home);
            ShowTimerView = new RelayCommand(Timer);
            ShowProjectView = new RelayCommand(Project);
            ShowTimesheetView = new RelayCommand(Timesheet);
            ShowUserView = new RelayCommand(UserV);
            ShowUserHistory= new RelayCommand(UserHistory);
            ShowSettingView = new RelayCommand(Setting);




            //CurrentChildView
            Home(null);


        }

        private void UserV(object obj)
        {
            CurrentChildView = new UserViewModel();
        }

        #region ICommand handlers

        private void Timesheet(object obj)
        {
            CurrentChildView = new TimeSheetViewModel();

        }
        private void Timer(object obj)
        {
            CurrentChildView = _timerViewModel;
            
        }

        private void Home(object obj)
        {
            CurrentChildView = new HomeViewModel();

        }

        private void UserHistory(object obj)
        {
            CurrentChildView = _userHistoryViewModel;

        }

        private void Project(object obj)
        {
            CurrentChildView = _projectViewModel;
        }

        private void Setting(object obj)
        {
            CurrentChildView = new SettingsViewModel();
            
        }

        private void CurrentUserData()
        {
            var user = GetUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                {
                    UserAccount.Username = user.Username;
                    UserAccount.DisplayName = $"{user.Username}";
                    
                };

            }
            else
            {
                userAccount.DisplayName = "Invalid, not logged in";
                
            }
        }

        #endregion

        #region Event rising fields

            private bool _btnUpdateVisibility;

        public bool btnUpdateVisibility
        {
            get { return _btnUpdateVisibility; }
            set { _btnUpdateVisibility = value; OnPropertyChanged(nameof(btnUpdateVisibility)); }


        }

        private bool _btnAdminVisi;

        public bool btnAdminVisi
        {
            get { return _btnAdminVisi; }
            set { _btnAdminVisi = value; OnPropertyChanged(nameof(btnAdminVisi)); }


        }

        public UserAccountModel UserAccount
        {
            get
            {
                return userAccount;
            }

            set
            {

                userAccount = value;
                OnPropertyChanged(nameof(UserAccount));
            }
        }
        public object CurrentChildView 
        { 
            get
            {
                return _CurrentChildView;
            }
            
            set
            {
                _CurrentChildView = value;
              OnPropertyChanged(nameof(CurrentChildView));  
            }
             
        }

        #endregion
        
    }
}
