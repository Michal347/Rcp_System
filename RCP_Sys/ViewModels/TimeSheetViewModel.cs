using RCP_Sys.Db;
using RCP_Sys.Models;
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

        #region Icommand 

        public ICommand RefreshTimes { get; private set; }
        #endregion


        public TimeSheetViewModel()
        {
            DataListView();
            TimeCollection = CollectionViewSource.GetDefaultView(TimeViewCollection);
            TimeCollection.Filter = TimeFilter;
        }

        private bool TimeFilter(object obj)
        {
            if (obj is TimerModel timermodel)
            {
                return timermodel.EndDateTime.Contains(Filter) ||
                    timermodel.Project.Contains(Filter) ||
                    timermodel.Username.Contains(Filter);
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
            set { _TimeViewCollection = value; OnPropertyChanged("TimeViewCollection"); }
        }

        private string _Filter = string.Empty;

        public string Filter
        {
            get
            {
                return _Filter;
            }
            set
            {

                _Filter = value;
                OnPropertyChanged("Filter");
                TimeCollection.Refresh();
            }
        }

    }
}
