using FontAwesome.Sharp;
using RCP_Sys.DAL.Interface;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Services;
using RCP_Sys.Utilities;
using RCP_Sys.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace RCP_Sys.ViewModels
{
    public class MainViewModel : BaseViewModel, INotifyPropertyChanged
    {


        public ICommand ShowHomeView { get; }
        public ICommand ShowTimerView { get; }
        public ICommand ShowProjectView { get; }
        public ICommand ShowTimesheetView { get; }
        public ICommand ShowUserView { get; }
        public ICommand ShowUserHistory { get; }
        public ICommand ShowSettingView { get; }
        public ICommand ExitProgram { get; }
        public ICommand ShowReportsView { get; }
        public ICommand ShowReportsUserView { get; }




        private UserAccountInformation userAccount;
        public IDialogService DialogAction;
        private IUserService GetUsername;
        private object _CurrentChildView;
        public TimerViewModel _timerViewModel;
        public UserViewModel _userViewModel;

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
            _userViewModel = new UserViewModel();
            GetUsername = new UserService();
            DialogAction = new DialogService();
            UserAccount = new UserAccountInformation();
            CurrentUserData();


            ShowHomeView = new RelayCommand(Home);
            ShowTimerView = new RelayCommand(Timer);
            ShowProjectView = new RelayCommand(Project);
            ShowTimesheetView = new RelayCommand(Timesheet);
            ShowUserView = new RelayCommand(UserV);
            ShowUserHistory = new RelayCommand(UserHistory);
            ShowSettingView = new RelayCommand(Setting);
            ExitProgram = new RelayCommand(Exit);
            ShowReportsView = new RelayCommand(Reports);
            ShowReportsUserView = new RelayCommand(ReportsUser);




            //Start program ChildView
            Home(null);


        }



        private void Exit(object obj)
        {
            if (_timerViewModel.IsTimerRunning == false)
            {
                Application.Current.Shutdown();
            }
            else
            {
                DialogAction.ShowDialog();
            }
        }


        private void UserV(object obj)
        {
            CurrentChildView = _userViewModel;
            CurrentUserData();
            caption = "User";
            icon = IconChar.UserCheck;

        }

        private void Timesheet(object obj)
        {
            CurrentChildView = new TimeSheetViewModel();
            CurrentUserData();
            caption = "Timesheet";
            icon = IconChar.Calendar;

        }
        private void Timer(object obj)
        {
            CurrentChildView = _timerViewModel;
            CurrentUserData();
            new MainViewModel();
            caption = "Timer";
            icon = IconChar.Clock;
            _timerViewModel.LoadProjects();
            _timerViewModel.DataGridTimer();

        }

        private void Home(object obj)
        {
            CurrentChildView = new HomeViewModel();
            caption = "Home";
            icon = IconChar.Home;
            CurrentUserData();
        }

        private void UserHistory(object obj)
        {
            CurrentChildView = new UserHistoryViewModel();
            CurrentUserData();
            new MainViewModel();
            caption = "User History";
            icon = IconChar.History;

        }

        private void Project(object obj)
        {
            CurrentChildView = new ProjectViewModel();
            CurrentUserData();
            caption = "Project";
            icon = IconChar.ProjectDiagram;

        }

        private void Setting(object obj)
        {
            CurrentChildView = new SettingsViewModel();
            caption = "Setting";
            icon = IconChar.Cog;

        }

        private void Reports(object obj)
        {
            CurrentChildView = new ReportsViewModel();
            caption = "Reports";
            icon = IconChar.AreaChart;
        }

        private void ReportsUser(object obj)
        {
            CurrentChildView = new ReportsUserViewModel();
            caption = "Reports";
            icon = IconChar.AreaChart;
        }

        public void CurrentUserData()
        {
            var user = GetUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                {
                    UserAccount.Username = user.Username;
                    UserAccount.DisplayName = $"{user.Name} {user.Surname}";

                };

            }
            else
            {
                userAccount.DisplayName = "Invalid, not logged in";

            }
        }




        private IconChar _icon;
        public IconChar icon
        {
            get { return _icon; }
            set { _icon = value; OnPropertyChanged(nameof(icon)); }


        }
        private string _caption;
        public string caption
        {
            get { return _caption; }
            set { _caption = value; OnPropertyChanged(nameof(caption)); }


        }

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

        public UserAccountInformation UserAccount
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

    }
}
