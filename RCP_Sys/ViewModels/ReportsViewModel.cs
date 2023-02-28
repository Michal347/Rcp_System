using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace RCP_Sys.ViewModels
{
    public class ReportsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ICommand EditCommand { get; }
        public IUserService GetUser;

        public ReportsViewModel()
        {
            LoadUsers();
            UserIcollection = CollectionViewSource.GetDefaultView(UserCollectionReports);
            EditCommand = new RelayCommand(EditUser);
            GetUser = new UserService();
        }

        private void EditUser(object obj)
        {
            using (var context = new RcpDbContext())
            {
                var date = DateTime.Today;
                var user = GetUser.GetUsers();
                var emp = obj as UserModel;
                if (emp != null)
                {
                    if (emp.Username != "Admin")
                    {
                        ///
                        User = emp.Username;
                     
                        /// join date 

                        DayJoin = emp.DateTimeJoined.ToString("MM/dd/yyyy");


                        ///Count project 
                        var Project = (from a in context.Times
                                    where a.Username == emp.Username                
                                    select a.Project).Distinct();

                      
                        ProjectCount= Project.Count();
                        ///Year

                        var Timer = from a in context.Times
                                    where a.Username == emp.Username
                                    && a.DateCreate.Year == DateTime.Today.Year
                                    select a;

                        YearTimeCollection = new ObservableCollection<TimerModel>(Timer);

                        ///Month
                        var Month = from a in context.Times
                                    where a.Username == emp.Username
                                    && a.DateCreate.Month == DateTime.Today.Month && a.DateCreate.Year == DateTime.Today.Year
                                    select a;

                        TimerUserCollection = new ObservableCollection<TimerModel>(Month);


                        ///Day 

                        var Days = from a in context.Times
                                    where a.Username == emp.Username
                                    && a.DateCreate.Day == DateTime.Today.Day && a.DateCreate.Year == DateTime.Today.Year
                                    select a;

                        DayTimeCollection = new ObservableCollection<TimerModel>(Days);
                        ///All Day
                        var Day = (from a in context.Users
                                     where a.Username == emp.Username
                                   select a.DateTimeJoined).FirstOrDefault();

                        DaysAtwork = date.Day - Day.Day;


                        ///////////////////////


                        SumDay = new TimeSpan((long)DayTimeCollection.Sum(p => p.EndTimerValue.Ticks));
                        DayFormatY = string.Format("{0}", SumDay.Days * 24 + SumDay.Hours);

                        SumMonth = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                        HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);


                        SumYear = new TimeSpan((long)YearTimeCollection.Sum(p => p.EndTimerValue.Ticks));
                        HourFormatY = string.Format("{0}", SumYear.Days * 24 + SumYear.Hours);
                    }
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
                UserCollectionReports = new ObservableCollection<UserModel>(q);

            }
        }


        private ObservableCollection<UserModel> _UserCollectionReports;

        public ObservableCollection<UserModel> UserCollectionReports
        {
            get { return _UserCollectionReports; }
            set { _UserCollectionReports = value; OnPropertyChanged("UserCollectionReports"); }
        }

        private ICollectionView _UserIcollection;
        public ICollectionView UserIcollection
        {
            get { return _UserIcollection; }
            set { _UserIcollection = value; OnPropertyChanged("UserIcollection"); }
        }

        private int _DaysAtWork;
        public int DaysAtwork
        {
            get
            {
                return _DaysAtWork;
            }
            set
            {
                _DaysAtWork = value; OnPropertyChanged(nameof(DaysAtwork));
            }
        }

        private TimeSpan _SumMonth;
        public TimeSpan SumMonth
        {
            get { return _SumMonth; }
            set { _SumMonth = value; OnPropertyChanged(nameof(SumMonth)); }
        }

        private TimeSpan _SumYear;
        public TimeSpan SumYear
        {
            get { return _SumYear; }
            set { _SumYear = value; OnPropertyChanged(nameof(SumYear)); }
        }


        private TimeSpan _SumDay;
        public TimeSpan SumDay
        {
            get { return _SumDay; }
            set { _SumDay = value; OnPropertyChanged(nameof(SumDay)); }
        }

        private ObservableCollection<TimerModel> _YearTimeCollection;

        public ObservableCollection<TimerModel> YearTimeCollection
        {
            get { return _YearTimeCollection; }
            set { _YearTimeCollection = value; OnPropertyChanged("YearTimeCollection"); }
        }

        private ObservableCollection<TimerModel> _DayTimeCollection;

        public ObservableCollection<TimerModel> DayTimeCollection
        {
            get { return _DayTimeCollection; }
            set { _DayTimeCollection = value; OnPropertyChanged("DayTimeCollection"); }
        }

        private ObservableCollection<TimerModel> _ProjectCollection;

        public ObservableCollection<TimerModel> ProjectCollection
        {
            get { return _ProjectCollection; }
            set { _ProjectCollection = value; OnPropertyChanged("ProjectCollection"); }
        }


        private string _HourFormat;
        public string HourFormat
        {
            get
            {
                return _HourFormat;
            }
            set
            {
                _HourFormat = value; OnPropertyChanged(nameof(HourFormat));
            }
        }

        private string _HourFormatY;
        public string HourFormatY
        {
            get
            {
                return _HourFormatY;
            }
            set
            {
                _HourFormatY = value; OnPropertyChanged(nameof(HourFormatY));
            }
        }

        private string _DayFormatY;
        public string DayFormatY
        {
            get
            {
                return _DayFormatY;
            }
            set
            {
                _DayFormatY = value; OnPropertyChanged(nameof(DayFormatY));
            }
        }

        private string _DayJoin;
        public string DayJoin
        {
            get
            {
                return _DayJoin;
            }
            set
            {
                _DayJoin = value; OnPropertyChanged(nameof(DayJoin));
            }
        }

        private string _User;
        public string User
        {
            get
            {
                return _User;
            }
            set
            {
                _User = value; OnPropertyChanged(nameof(User));
            }
        }

        private int _ProjectCount;
        public int ProjectCount
        {
            get
            {
                return _ProjectCount;
            }
            set
            {
                _ProjectCount = value; OnPropertyChanged(nameof(ProjectCount));
            }
        }

        private ObservableCollection<TimerModel> _TimerUserCollection;

        public ObservableCollection<TimerModel> TimerUserCollection
        {
            get { return _TimerUserCollection; }
            set { _TimerUserCollection = value; OnPropertyChanged("TimerUserCollection"); }
        }

    }
}
