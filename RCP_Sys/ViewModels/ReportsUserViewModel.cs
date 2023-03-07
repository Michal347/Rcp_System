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
        private IUserService getUsername;

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
            TimesMonth();
            TimesMonthAdd1();
            TimesMonthAdd2();
            TimesMonthAdd3();
            TimesMonthAdd4();
            getUsername = new UserService();
            SetChartModelValues();

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

        private ObservableCollection<TimerModel> _TimerUserCollection;

        public ObservableCollection<TimerModel> TimerUserCollection
        {
            get { return _TimerUserCollection; }
            set { _TimerUserCollection = value; OnPropertyChanged(nameof(TimerUserCollection)); }
        }

        private ObservableCollection<TimerModel> _TimerUserCollection1;

        public ObservableCollection<TimerModel> TimerUserCollection1
        {
            get { return _TimerUserCollection1; }
            set { _TimerUserCollection1 = value; OnPropertyChanged(nameof(TimerUserCollection1)); }
        }


        private ObservableCollection<TimerModel> _TimerUserCollection2;

        public ObservableCollection<TimerModel> TimerUserCollection2
        {
            get { return _TimerUserCollection2; }
            set { _TimerUserCollection2 = value; OnPropertyChanged(nameof(TimerUserCollection2)); }
        }


        private ObservableCollection<TimerModel> _TimerUserCollection3;

        public ObservableCollection<TimerModel> TimerUserCollection3
        {
            get { return _TimerUserCollection3; }
            set { _TimerUserCollection3 = value; OnPropertyChanged(nameof(TimerUserCollection3)); }
        }


        private ObservableCollection<TimerModel> _TimerUserCollection4;

        public ObservableCollection<TimerModel> TimerUserCollection4
        {
            get { return _TimerUserCollection4; }
            set { _TimerUserCollection4 = value; OnPropertyChanged(nameof(TimerUserCollection4)); }
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

                TimerUserCollection1 = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd1 = new TimeSpan((long)TimerUserCollection1.Sum(p => p.EndTimerValue.Ticks));
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

                TimerUserCollection2 = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd2 = new TimeSpan((long)TimerUserCollection2.Sum(p => p.EndTimerValue.Ticks));
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

                TimerUserCollection3 = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd3 = new TimeSpan((long)TimerUserCollection3.Sum(p => p.EndTimerValue.Ticks));
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

                TimerUserCollection4 = new ObservableCollection<TimerModel>(Timer);

                SumMonthAdd4 = new TimeSpan((long)TimerUserCollection4.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);

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
    }
}


