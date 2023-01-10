using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Win32;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;

namespace RCP_Sys.ViewModels
{
    public class ProjectViewModel : BaseViewModel, INotifyPropertyChanged
    {


        #region Icommand

        public ICommand AddProject { get; private set; }
        public ICommand RefreshProjects { get; private set; }


        #endregion


        #region Ctors
        public ProjectViewModel()
        {

            AddProject = new RelayCommand(x => AddProj());
            RefreshProjects = new RelayCommand(x => Refresh());

        }

        #endregion

        #region ICommand handlers
        private void AddProj()
        {
            {
                using (var context = new RcpDbContext())
                {
                    var found = context.Projects.FirstOrDefault(x => x.Name == ProjectName);
                    if (found != null)
                    {

                        OnErrorCreated("ProjectName", "*Project already exist");
                        return;

                    }
                    else if (string.IsNullOrWhiteSpace(ProjectName) || string.IsNullOrEmpty(Description))
                    {
                        OnErrorCreated("ProjectName", "*Fill the empty fields");
                        OnErrorCreated("Description", "*Fill the empty fields");
                    }
                    else if (ProjectName != string.Empty || Description != string.Empty)
                    {
                        context.Projects.Add(
                    new ProjectModel()
                    {

                        Name = ProjectName,
                        Description = Description,

                    }
                    );
                        {
                            context.SaveChanges();
                            MysampleGrid = context.Projects.Where(x => x.Name != null).ToList();
                            ErrorMessage = "";
                        }
                    }

                }

            }

        }

        private void Refresh()
        {
            using (var context = new RcpDbContext())
            {
                MysampleGrid = context.Projects.Where(x => x.Name != null).ToList();
            }

        }


        #endregion

        #region fields

        private string _projectName;

        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged("ProjectName");
                ClearPropertyErrors(this, "ProjectName");

            }
        }

        private string _descripiton;

        public string Description
        {
            get { return _descripiton; }
            set
            {
                _descripiton = value;
                OnPropertyChanged("Description");
                ClearPropertyErrors(this, "Description");

            }
        }



        private string _selectedDataGridItem;

        public string SelectedDataGridItem
        {
            get { return _selectedDataGridItem; }
            set
            {
                _selectedDataGridItem = value;
                OnPropertyChanged("SelectedDataGridItem");

            }
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private List<ProjectModel> mysampleGrid;

        public List<ProjectModel> MysampleGrid
        {
            get { return mysampleGrid; }
            set { mysampleGrid = value; OnPropertyChanged("MysampleGrid"); }
        }


    }
    #endregion


}
