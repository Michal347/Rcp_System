using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.SqlServer;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace RCP_Sys.ViewModels
{
    public class ReportsUserViewModel :BaseViewModel, INotifyPropertyChanged
    {
        public SeriesCollection SeriesCollection { get; set; }
        public DateTime InitialDateTime { get; set; }
        public Func<double, string> Formatter { get; set; }
        public PeriodUnits Period { get; set; }
        public IAxisWindow SelectedWindow { get; set; }
        public ICommand ResultCommand { get; set; }

        public class ChartModel
        {
            public DateTime DateTime { get; set; }
            public double Value { get; set; }

            public ChartModel(DateTime dateTime, double value)
            {
                this.DateTime = dateTime;
                this.Value = value;
            }
        }


        public ReportsUserViewModel()
        {
            UserData();
            TimesMonth();
            TimesMonthAdd1();
            TimesMonthAdd2();
            TimesMonthAdd3();
            TimesMonthAdd4();
            SetChartModelValues();
            ResultCommand = new RelayCommand(x=>ResultPrize());

        }

        private void SetChartModelValues()
        {
            var dayConfig = Mappers.Xy<ChartModel>()
                               .X(dayModel => dayModel.DateTime.Ticks)
                               .Y(dayModel => dayModel.Value);

            DateTime now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);


            this.SeriesCollection = new SeriesCollection(dayConfig)
        {
            new LineSeries()
            {
                Title = "Hour",

                Values = new ChartValues<ChartModel>()
                {
                    new ChartModel(now.AddMonths(-4), SumMonthAdd4.TotalHours),
                    new ChartModel(now.AddMonths(-3), SumMonthAdd3.TotalHours),
                    new ChartModel(now.AddMonths(-2), SumMonthAdd2.TotalHours),
                    new ChartModel(now.AddMonths(-1), SumMonthAdd1.TotalHours),
                    new ChartModel(now, SumMonth.TotalHours),
                   
                }
            }
        };

            this.Formatter = value => new DateTime((long)value).ToString("yyyy-MM");
        }

        

        public void TimesMonth()
        {
           
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name
                            && a.DateCreate.Month == DateTime.Today.Month
                            select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);
                SumMonth = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

            }
        }


        public void TimesMonthAdd1()
        {
            var date = DateTime.Today.AddMonths(-1);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                             where a.Username == Thread.CurrentPrincipal.Identity.Name &&
                             a.DateCreate.Month == date.Month
                             select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd1 = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

            }
        }

        public void TimesMonthAdd2()
        {
            var date = DateTime.Today.AddMonths(-2);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name &&
                            a.DateCreate.Month == date.Month
                            select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd2 = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

            }
        }

        public void TimesMonthAdd3()
        {
            var date = DateTime.Today.AddMonths(-3);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                           where a.Username == Thread.CurrentPrincipal.Identity.Name &&                         
                            a.DateCreate.Month == date.Month
                            select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd3 = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

            }
        }
        public void TimesMonthAdd4()
        {
            var date = DateTime.Today.AddMonths(-4);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name &&
                            a.DateCreate.Month == date.Month
                            select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd4 = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

            }
        }

        public void ResultPrize()
        {
          Result = HourCount * Prize;
        }

        private void UserData()
        {
            using (var context = new RcpDbContext())
            {
                var date = DateTime.Today;

                /// join date 

                var Joindate = (from a in context.Users
                                where a.Username == Thread.CurrentPrincipal.Identity.Name
                                select a.DateTimeJoined).FirstOrDefault();

                DayJoin = Joindate.ToString("MM/dd/yyyy");

                ///Count project 
                var Project = (from a in context.Times
                                       where a.Username == Thread.CurrentPrincipal.Identity.Name
                                       select a.Project).Distinct();


                        ProjectCount = Project.Count();
                        ///Year

                        var Timer = from a in context.Times
                                    where a.Username == Thread.CurrentPrincipal.Identity.Name
                                    && a.DateCreate.Year == DateTime.Today.Year
                                    select a;

                        YearTimeCollection = new ObservableCollection<TimerModel>(Timer);

                        ///Month
                        var Month = from a in context.Times
                                    where a.Username == Thread.CurrentPrincipal.Identity.Name
                                    && a.DateCreate.Month == DateTime.Today.Month && a.DateCreate.Year == DateTime.Today.Year
                                    select a;

                        TimerUserCollection = new ObservableCollection<TimerModel>(Month);


                        ///Day 

                        var Days = from a in context.Times
                                   where a.Username == Thread.CurrentPrincipal.Identity.Name
                                   && a.DateCreate.Day == DateTime.Today.Day && a.DateCreate.Year == DateTime.Today.Year
                                   select a;

                        DayTimeCollection = new ObservableCollection<TimerModel>(Days);
                        ///All Day
                        var Day = (from a in context.Users
                                   where a.Username == Thread.CurrentPrincipal.Identity.Name
                                   select a.DateTimeJoined).FirstOrDefault();

                        DaysAtwork = (int)(Day - date).TotalDays;


                        ///////////////////////


                        SumDay = new TimeSpan((long)DayTimeCollection.Sum(p => p.EndTimerValue.Ticks));
                        DayFormatY = string.Format("{0}", SumDay.Days * 24 + SumDay.Hours);

                        SumMonth = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                        HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);


                        SumYear = new TimeSpan((long)YearTimeCollection.Sum(p => p.EndTimerValue.Ticks));
                        HourFormatY = string.Format("{0}", SumYear.Days * 24 + SumYear.Hours);
                   
            }
        }

        private TimeSpan _SumMonth;
        public TimeSpan SumMonth
        {
            get { return _SumMonth; }
            set { _SumMonth = value; OnPropertyChanged(nameof(SumMonth));}
        }

        private TimeSpan _SumMonthAdd1;
        public TimeSpan SumMonthAdd1
        {
            get { return _SumMonthAdd1; }
            set { _SumMonthAdd1 = value; OnPropertyChanged(nameof(SumMonthAdd1)); }
        }

        private TimeSpan _SumMonthAdd2;
        public TimeSpan SumMonthAdd2
        {
            get { return _SumMonthAdd2; }
            set { _SumMonthAdd2 = value; OnPropertyChanged(nameof(SumMonthAdd2)); }
        }

        private TimeSpan _SumMonthAdd3;
        public TimeSpan SumMonthAdd3
        {
            get { return _SumMonthAdd3; }
            set { _SumMonthAdd3 = value; OnPropertyChanged(nameof(SumMonthAdd3)); }
        }

        private TimeSpan _SumMonthAdd4;
        public TimeSpan SumMonthAdd4
        {
            get { return _SumMonthAdd4; }
            set { _SumMonthAdd4 = value; OnPropertyChanged(nameof(SumMonthAdd4)); }
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
            set { _YearTimeCollection = value; OnPropertyChanged(nameof(YearTimeCollection)); }
        }

        private ObservableCollection<TimerModel> _DayTimeCollection;

        public ObservableCollection<TimerModel> DayTimeCollection
        {
            get { return _DayTimeCollection; }
            set { _DayTimeCollection = value; OnPropertyChanged(nameof(DayTimeCollection)); }
        }

        private ObservableCollection<TimerModel> _ProjectCollection;

        public ObservableCollection<TimerModel> ProjectCollection
        {
            get { return _ProjectCollection; }
            set { _ProjectCollection = value; OnPropertyChanged(nameof(ProjectCollection)); }
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


        private int _HourCount;
        public int HourCount
        {
            get
            {
                return _HourCount;
            }
            set
            {
                _HourCount = value; OnPropertyChanged(nameof(HourCount));
            }
        }

        private double _Prize;
        public double Prize
        {
            get
            {
                return _Prize;
            }
            set
            {
                _Prize = value; OnPropertyChanged(nameof(Prize));
            }
        }

        private double _Result;
        public double Result
        {
            get
            {
                return _Result;
            }
            set
            {
                _Result = value; OnPropertyChanged(nameof(Result));
            }
        }

        private ObservableCollection<TimerModel> _TimerUserCollection;

        public ObservableCollection<TimerModel> TimerUserCollection
        {
            get { return _TimerUserCollection; }
            set { _TimerUserCollection = value; OnPropertyChanged(nameof(TimerUserCollection)); }
        }

    }
}


