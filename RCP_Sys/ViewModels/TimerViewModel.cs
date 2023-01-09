using RCP_Sys.Db;
using System;
using RCP_Sys.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using RCP_Sys.Repository;
using System.Data.Entity.Migrations;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using RCP_Sys.Models.Interface;

namespace RCP_Sys.ViewModels
{
    public class TimerViewModel : BaseViewModel, INotifyPropertyChanged
    {

        #region Icommand

        public ICommand StartTimerCommand { get; set; }
        public ICommand StopTimerCommand { get; set; }
        public ICommand Refresh {get; set; }
        public ICommand RefreshDataGrid { get; set; }
        private IUserService getUsername;
        private ITimerService TimeCreate;

        #endregion

        public CancellationTokenSource _cancellationTokenSource;
        public SynchronizationContext synchronizationContext = SynchronizationContext.Current;


          public TimerViewModel()
            {
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
            Refresh = new RelayCommand(RefreshProject);
            RefreshDataGrid = new RelayCommand(RefreshDataGridTimer);
            getUsername = new UserService();
            TimeCreate = new TimerService();
            //check if database has start date but no end date
            //if started but not stopped then start with added value

            //if not started and not stopped or started and stoped then start with 0

            bool hasBeenStarted = false; //for test puproses, should be retrieved from database

            if (hasBeenStarted)
            {
                IsTimerRunning = true;
                var databaseDate = DateTime.Now; //mock for testing
                TimerBoxValue = DateTime.Now.Subtract(databaseDate);
                _cancellationTokenSource = new CancellationTokenSource();
                StartTimerLoop();

            }
            else
            {
                IsTimerRunning = false;
                TimerBoxValue = TimeSpan.Zero;
            }


            LoadProjects();
            }

      
        #region ICommand handlers
        private void StartTimerLoop()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            IsTimerRunning = true;
            Task.Factory.StartNew(TimerLoop, _cancellationTokenSource.Token, _cancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        private async Task TimerLoop(object state)
        {
            var cancellationToken = (CancellationToken)state;
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
                await Task.Run(() =>
                {
                    SynchronizationContext.SetSynchronizationContext(synchronizationContext);
                    TimerBoxValue = TimerBoxValue.Add(TimeSpan.FromSeconds(1));
                });

                await Task.Delay(1000);
            }
        }

        private void StopTimer(object obj)
        {
            DateTime aDate = DateTime.Now;
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);

                using (var context = new RcpDbContext())
                {

                        TimeCreate.Create(
                        new TimerModel()
                        {
                            Username = user.Username,
                            StartDateTime = DateTime.Today,
                            EndDateTime = aDate.ToString("MM/dd/yyyy hh:mm tt"),
                            StartTimerValue = TimeSpan.Zero,
                            EndTimerValue = TimerBoxValue,
                            Project = SelectedProject1,
                            Description = selectedDescription,
                            DateCreate = DateTime.Today,

                        }
                    );
     
            }

            IsVisible = true;
            IsTimerRunning = false;
            selectedDescription= string.Empty;
            SelectedProject1 = string.Empty;
            TimerBoxValue = TimeSpan.Zero;
            _cancellationTokenSource.Cancel();
            RefreshDataGridTimer(obj);
        }

        private void StartTimer(object obj)
        {
            if (string.IsNullOrWhiteSpace(SelectedProject1))
            {
                OnErrorCreated("SelectedProject1", "*Select Project");
                return;
            }

            StartTimerLoop();
            IsVisible = false;
        }

        private void RefreshProject(object obj)
        {
            LoadProjects();
        }

        private void RefreshDataGridTimer(object obj)
        {
            DataGridTimer();
        }

        public void LoadProjects()
        {
            using (var context = new RcpDbContext())
            {
                var q = from s in context.Projects
                         select s;

                ProjectCollection = new ObservableCollection<ProjectModel>(q);

            }
        }

        public void DataGridTimer()
        {

            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            using (var context = new RcpDbContext())
            {
                
              
                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name &&
                            a.DateCreate== DateTime.Today
                            select a;

                TimerCollection = new ObservableCollection<TimerModel>(Timer);

            }
        }

        #endregion

        #region Event Fields


        private bool _isTimerRunning;

        public bool IsTimerRunning
        {
            get { return _isTimerRunning; }
            set { _isTimerRunning = value; OnPropertyChanged(nameof(IsTimerRunning)); }
        }


            private string selectedProject;

        public string SelectedProject1
        {
            get
            {
                return selectedProject;
            }
            set
            {
                selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject1));
                ClearPropertyErrors(this, "SelectedProject1");
            }
        }

            private TimeSpan _timerBoxValue;

        public TimeSpan TimerBoxValue
        {
            get { return _timerBoxValue; }
            set { _timerBoxValue = value; OnPropertyChanged(nameof(TimerBoxValue)); }
        }


            private string SelectedDescription;

        public string selectedDescription
        {
            get
            {
                return SelectedDescription;
            }
            set { SelectedDescription = value; OnPropertyChanged(nameof(selectedDescription)); }
        }

        private bool _IsVisible = true;
        public bool IsVisible
        {
            get => _IsVisible;
            set
            {
                _IsVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }

        }
        #endregion

        #region Collection

            private ObservableCollection<ProjectModel> _projectCollection;

        public ObservableCollection<ProjectModel> ProjectCollection
        {
            get { return _projectCollection; }
            set { _projectCollection = value; OnPropertyChanged("ProjectCollection"); }
        }

            private ObservableCollection<TimerModel> _TimerCollection;

        public ObservableCollection<TimerModel> TimerCollection
        {
            get { return _TimerCollection; }
            set { _TimerCollection = value; OnPropertyChanged("TimerCollection"); }
        }

        #endregion
    }
}
