
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RCP_Sys.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private UserAccountInformation _UserInformation;
        private IUserService getUsername;
        private IUserService getGender;
        public ICommand OpenFile { get; set; }

        public UserAccountInformation UserInformation
        {
            get
            {
                return _UserInformation;
            }
            set
            {
                _UserInformation = value;
                OnPropertyChanged(nameof(UserInformation));
            }
        }

        public HomeViewModel()
        {
            getUsername = new UserService();
            getGender = new UserService();
            UserInformation = new UserAccountInformation();
            CurrentUserInformation();      
            LoadImage();
            TimesMonth();
            TimesYear();
            WorkDay();
            YearTimeSum();
            HometimeSum();
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                if (user.IsUserAdmin == false)
                {
                    Position = "Status: User";
                }
                else
                {
                    Position = "Status: Administrator";
                }
            }
        }

        public void LoadImage()
        {

            using (var context = new RcpDbContext())
            {

                var q = (from s in context.Picture
                         where s.Username == Thread.CurrentPrincipal.Identity.Name
                         select s.ImageToByte).FirstOrDefault();

                if (q != null)
                {
                    Stream StreamObj = new MemoryStream(q);

                    BitmapImage BitObj = new BitmapImage();

                    BitObj.BeginInit();

                    BitObj.StreamSource = StreamObj;

                    BitObj.EndInit();

                    this.ImageSource = BitObj;
                }
                else
                {
                    var gender = getGender.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                    if (gender != null)
                    {
                        var value = "Female";
                        Boolean result = gender.Gender.Contains(value);
                        if (result == true)
                        {
                            string imagePath = "../Images/woman.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }
                        if (result == false)
                        {
                            string imagePath = "../Images/man.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }

                        if (result == false && gender.IsUserAdmin == true)
                        {
                            string imagePath = "../Images/businessman.png";
                            this.ImageSource = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                        }
                    }
                }
            }
        }



                public void CurrentUserInformation()
                {
                    var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
                    if (user != null)
                    {
                        {
                            UserInformation.Username = user.Username;
                            UserInformation.Name = user.Name;
                            UserInformation.Surname = user.Surname;
                            UserInformation.DateJoin = user.DateTimeJoined.ToString("MM/dd/yyyy");
                            UserInformation.Email = user.Email;


                        };


                    }
                    else
                    {
                        UserInformation.Username = "Invalid, Information";
                        UserInformation.Name = "Invalid, Information";
                        UserInformation.Surname = "Invalid, Information";
                        UserInformation.Email = "Invalid, Information";
                        UserInformation.DateJoin = "Invalid, Information";

                    }
                }
            
            private BitmapImage _ImageSource;
        public BitmapImage ImageSource
        {
            get { return this._ImageSource; }
            set { this._ImageSource = value; this.OnPropertyChanged("ImageSource"); }
        }


        private string _Position;
        public string Position
        {
            get
            {
                return _Position;

            }

            set
            {
                _Position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        public void HometimeSum()
        {    
                SumMonth = new TimeSpan((long)TimerUserCollection.Sum(p => p.EndTimerValue.Ticks));
                HourFormat = string.Format("{0}", SumMonth.Days * 24 + SumMonth.Hours);
        }

        public void WorkDay()
        {
            var date = DateTime.Today;
            using (var context = new RcpDbContext())
            {


                var Timer = (from a in context.Users
                            where a.Username == Thread.CurrentPrincipal.Identity.Name
                            select a.DateTimeJoined).FirstOrDefault();

                DaysAtwork = date.Day - Timer.Day;
            }

            
        }

        public void YearTimeSum()
        {
            SumYear = new TimeSpan((long)YearTimeCollection.Sum(p => p.EndTimerValue.Ticks));
            HourFormatY = string.Format("{0}", SumYear.Days * 24 + SumYear.Hours);
        }

        public void TimesMonth()
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                             where a.Username == Thread.CurrentPrincipal.Identity.Name
                             && a.DateCreate.Month == DateTime.Today.Month && a.DateCreate.Year == DateTime.Today.Year
                            select a;

                TimerUserCollection = new ObservableCollection<TimerModel>(Timer);

            }
        }


        public void TimesYear()
        {
            var user = getUsername.GetUserModels(Thread.CurrentPrincipal.Identity.Name);
            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name
                            && a.DateCreate.Year == DateTime.Today.Year
                            select a;

                YearTimeCollection = new ObservableCollection<TimerModel>(Timer);

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


        private ObservableCollection<TimerModel> _TimerUserCollection;

        public ObservableCollection<TimerModel> TimerUserCollection
        {
            get { return _TimerUserCollection; }
            set { _TimerUserCollection = value; OnPropertyChanged("TimerUserCollection"); }
        }

        private ObservableCollection<TimerModel> _YearTimeCollection;

        public ObservableCollection<TimerModel> YearTimeCollection
        {
            get { return _YearTimeCollection; }
            set { _YearTimeCollection = value; OnPropertyChanged("YearTimeCollection"); }
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
    }
   

}
