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
    public class UserHistoryViewModel: BaseViewModel
    {

        public ICommand RefreshTimes { get; private set; }


        public UserHistoryViewModel()
        {
            DataListView();
            TimeCollectionUser = CollectionViewSource.GetDefaultView(TimeViewCollectionUser);
            TimeCollectionUser.Filter = TimeFilter;
            
        }

        private bool TimeFilter(object obj)
        {
            if (obj is TimerModel timermodel)
            {
                return  timermodel.EndDateTime.Contains(Filter) ||
                        timermodel.Project.Contains(Filter)||
                        timermodel.Username.Contains(Filter);
            }

            return false;

        }

       
        public void DataListView()
        {

            using (var context = new RcpDbContext())
            {


                var Timer = from a in context.Times
                            where a.Username != null
                            select a;

                TimeViewCollectionUser = new ObservableCollection<TimerModel>(Timer);

            }
        }
        public ICollectionView TimeCollectionUser { get; set; }

        private ObservableCollection<TimerModel> _TimeViewCollection;

        public ObservableCollection<TimerModel> TimeViewCollectionUser
        {
            get { return _TimeViewCollection; }
            set { _TimeViewCollection = value; OnPropertyChanged("TimeViewCollectionUser"); }
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
                TimeCollectionUser.Refresh();
            }
        }

        private string _FilterUsername = string.Empty;

        public string FilterUsername
        {
            get
            {
                return _FilterUsername;
            }
            set
            {

                _FilterUsername = value;
                OnPropertyChanged("FilterUsername");
                TimeCollectionUser.Refresh();
            }
        }

    }
}

