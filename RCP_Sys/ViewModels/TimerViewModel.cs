using RCP_Sys.Db;
using System;
using RCP_Sys.Models;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;
using RCP_Sys.Repository;
using System.Collections.ObjectModel;
using RCP_Sys.Models.Interface;
using System.Runtime.Remoting.Contexts;
using System.Xml;
using System.Windows.Media;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace RCP_Sys.ViewModels
{
    public class TimerViewModel : BaseViewModel, INotifyPropertyChanged
    {



        public ICommand StartTimerCommand { get; set; }
        public ICommand StopTimerCommand { get; set; }
        public ICommand RefreshDataGrid { get; set; }
        private IUserService getUsername;
        private ITimerService TimeCreate;



        public CancellationTokenSource _cancellationTokenSource;
        public SynchronizationContext synchronizationContext = SynchronizationContext.Current;


        public TimerViewModel()
        {
            StartTimerCommand = new RelayCommand(StartTimer);
            StopTimerCommand = new RelayCommand(StopTimer);
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


            DataGridTimer();
            LoadProjects();
            DaytimeSum();     
        }


      


        private void DaytimeSum()
        {
            DayTime = new TimeSpan(TimerCollection.Sum(p => p.EndTimerValue.Ticks));
        }

        private void StartTimerLoop()
        {      
            DateTime aDate = DateTime.Now;
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            TimeCreate.Create(
            new TimerModel()
            {
                Username = user.Username,
                StartDateTime = aDate.ToString("MM/dd/yyyy hh:mm tt"),
                StartTimerValue = TimerBoxValue,
                Project = SelectedProject1,
                Description = selectedDescription,
                EndDateTime = "           --------",
                DateCreate = DateTime.Today,
            }
        ) ;
            DataGridTimer();
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
                    TimerBoxValue = TimerBoxValue.Add(TimeSpan.FromHours(4));
                });

                await Task.Delay(1000);
            }
        }

        private void StopTimer(object obj)
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            DateTime aDate = DateTime.Now;
            using (var context = new RcpDbContext())
            {
                MysampleGrid = context.Times.Where(x => x.Username == Thread.CurrentPrincipal.Identity.Name).ToList();
                var found = MysampleGrid.LastOrDefault();
                if(found!=null)
                {
                    found.EndTimerValue = TimerBoxValue;
                    found.EndDateTime = aDate.ToString("MM/dd/yyyy hh:mm tt");
                    context.Times.Update(found);
                    context.SaveChanges();
                }
            }

            IsVisible = true;
            IsTimerRunning = false;
            selectedDescription = string.Empty;
            SelectedProject1 = string.Empty;
            TimerBoxValue = TimeSpan.Zero;
            _cancellationTokenSource.Cancel();
            RefreshDataGridTimer(obj);
            DaytimeSum();
        }

        private void StartTimer(object obj)
        {
            if (string.IsNullOrWhiteSpace(SelectedProject1))
            {
                OnErrorCreated("SelectedProject1", "*Select Project");
                return;
            }

            StartTimerLoop();
            IsVisible = true;
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
                            a.DateCreate == DateTime.Today
                            select a;

                TimerCollection = new ObservableCollection<TimerModel>(Timer);

            }
        }



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
                ClearPropertyErrors(this, nameof(SelectedProject1));
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
            private TimeSpan _DayTime;
        public TimeSpan DayTime
        {
            get { return _DayTime; }
            set { _DayTime = value; OnPropertyChanged(nameof(DayTime)); }
        }



            private ObservableCollection<ProjectModel> _projectCollection;

        public ObservableCollection<ProjectModel> ProjectCollection
        {
            get { return _projectCollection; }
            set { _projectCollection = value; OnPropertyChanged(nameof(ProjectCollection)); }
        }

            private ObservableCollection<TimerModel> _TimerCollection;

        public ObservableCollection<TimerModel> TimerCollection
        {
            get { return _TimerCollection; }
            set { _TimerCollection = value; OnPropertyChanged(nameof(TimerCollection)); }
        }


        private List<TimerModel> mysampleGrid;

        public List<TimerModel> MysampleGrid
        {
            get { return mysampleGrid; }
            set { mysampleGrid = value; OnPropertyChanged(nameof(MysampleGrid)); }
        }


    }
}
