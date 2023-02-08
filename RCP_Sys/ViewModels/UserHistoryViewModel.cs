﻿using RCP_Sys.Db;
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

            IsFilterProjectVisi = true;
            IsFilterNameVisi = false;
            IsFilterDateVisi = true;


            if (IsCheckedProject == true)
            {
                IsFilterProjectVisi = false;
                IsFilterDateVisi = true;
                IsFilterNameVisi = true;
                TimeCollectionUser.Filter = TimeFilter;

            }
            if (IsCheckedName == true)
            {
                IsFilterProjectVisi = true;
                IsFilterDateVisi = true;
                IsFilterNameVisi = false;
                TimeCollectionUser.Filter = FilterName;

            }
            if (IsCheckedDate == true)
            {
                IsFilterProjectVisi = true;
                IsFilterDateVisi = false;
                IsFilterNameVisi = true;
                TimeCollectionUser.Filter = FilterDate;

            }

        }


        private bool FilterName(object obj)
        {
            if (obj is TimerModel timermodel)
            {
                return timermodel.Username.Contains(FilterUsername);
            
            }
            return false;
        }

        private bool FilterDate(object obj)
        {
            if (obj is TimerModel timermodel)
            {
                return timermodel.EndDateTime.Contains(FilterDates);

                
            }
            return false;
        }

        private bool TimeFilter(object obj)
        {
            if (obj is TimerModel timermodel)
            {
                 return timermodel.Project.Contains(Filter);    
                        
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
                OnPropertyChanged(nameof(Filter));
                TimeCollectionUser.Refresh();
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
                OnPropertyChanged(nameof(FilterUsername));
                TimeCollectionUser.Refresh();
            }
        }

            private bool _isCheckedName;
        public bool IsCheckedName
        {
            get
            {
                return _isCheckedName;
            }
            set
            {
                {
                    _isCheckedName = value;
                    OnPropertyChanged(nameof(IsCheckedName));

                }
            }
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


         private bool _isFilterProjectVisi;

        public bool IsFilterProjectVisi
        {
            get { return _isFilterProjectVisi; }
            set { _isFilterProjectVisi = value; OnPropertyChanged(nameof(IsFilterProjectVisi)); }
        }


            private bool _isFilterDateVisi;

        public bool IsFilterDateVisi
        {
            get { return _isFilterDateVisi; }
            set { _isFilterDateVisi = value; OnPropertyChanged(nameof(IsFilterDateVisi)); }
        }


         private bool _isFilterNameVisi;
            
        public bool IsFilterNameVisi
        {
            get { return _isFilterNameVisi; }
            set { _isFilterNameVisi = value; OnPropertyChanged(nameof(IsFilterNameVisi)); }
        }


    }
}

