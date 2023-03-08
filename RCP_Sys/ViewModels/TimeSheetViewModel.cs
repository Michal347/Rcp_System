using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Utilities;
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
    public class TimeSheetViewModel : BaseViewModel
    {



        public ICommand RefreshTimes { get; private set; }

        public TimeSheetViewModel()
        {
            DataListView();
            TimeCollection = CollectionViewSource.GetDefaultView(TimeViewCollection);
            GroupFilter gf = new GroupFilter();
            gf.AddFilter(ProjectFilter);
            gf.AddFilter(FilterDate);
            TimeCollection.Filter = gf.Filter;
            IsCheckedDate = false;
            IsCheckedProject = true;
        }



        private bool FilterDate(object obj)
        {

            if (obj is TimerModel timermodel)
            {
                return timermodel.EndDateTime.Contains(FilterDates);
            }
            return false;
        }

        private bool ProjectFilter(object obj)
        {
            if (obj is TimerModel timermodel)
            {

                return timermodel.Project.Contains(FilterProject);

            }

            return false;

        }
        public void DataListView()
        {

            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username == Thread.CurrentPrincipal.Identity.Name
                            select a;

                TimeViewCollection = new ObservableCollection<TimerModel>(Timer);

            }
        }
        public ICollectionView TimeCollection { get; set; }

        private ObservableCollection<TimerModel> _TimeViewCollection;

        public ObservableCollection<TimerModel> TimeViewCollection
        {
            get { return _TimeViewCollection; }
            set { _TimeViewCollection = value; OnPropertyChanged(nameof(TimeViewCollection)); }
        }

        

            private bool _isCheckedProject;
        public bool IsCheckedProject
        {
            get
            {
                return _isCheckedProject;
            }
            set
            {
                {
                    _isCheckedProject = value;
                    OnPropertyChanged(nameof(IsCheckedProject));

                }
            }
        }

            private bool _isCheckedDate;
        public bool IsCheckedDate
        {
            get
            {
                return _isCheckedDate;
            }
            set
            {
                {
                    _isCheckedDate = value;
                    OnPropertyChanged(nameof(IsCheckedDate));

                }
            }
        }


        private string _FilterProject = string.Empty;

        public string FilterProject
        {
            get
            {
                return _FilterProject;
            }
            set
            {

                _FilterProject = value;
                OnPropertyChanged(nameof(FilterProject));
                TimeCollection.Refresh();
            }
        }
        private string _FilterDates = string.Empty;

        public string FilterDates
        {
            get
            {
                return _FilterDates;
            }
            set
            {

                _FilterDates = value;
                OnPropertyChanged(nameof(FilterDates));
                TimeCollection.Refresh();
            }
        }

    }
}
