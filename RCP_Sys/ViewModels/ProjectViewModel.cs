using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Input;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Views;

namespace RCP_Sys.ViewModels
{
    public class ProjectViewModel : BaseViewModel, INotifyPropertyChanged
    {


        #region Icommand

        public ICommand AddProject { get; private set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        #endregion


        #region Ctors
        public ProjectViewModel()
        {

            AddProject = new RelayCommand(x => AddProj());
            DeleteCommand = new RelayCommand(Delete);
            EditCommand = new RelayCommand(Edit);
            UpdateCommand = new RelayCommand(Update);
            Refresh();

        }

       


        #endregion

        #region ICommand handlers
        private void AddProj()
        {
            ClearPropertyErrors(this, "ProjectName");       
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

        private void Delete(object obj)
        {
            using (var context = new RcpDbContext())
            {
                var emp = obj as ProjectModel;
                if (emp != null)
                {
                    context.Projects.Remove(emp);
                    MysampleGrid.Remove(emp);
                    context.SaveChanges();
                    Refresh();
                }

            }
        }

        private void Edit(object obj)
        {
            using (var context = new RcpDbContext())
            {


                var emp = obj as ProjectModel;
                if (emp != null)
                {
                    var dialog = new EditProjectDialog()
                    {
                        DataContext = this,
                    };
                    dialog.Show();

                    EditProjectName = emp.Name;
                    ID = emp.Id;
                    EditDescription = emp.Description;

                }
            }

        }

        private void Update(object obj)
        {
            ClearPropertyErrors(this, "EditProjectName");
            using (var context = new RcpDbContext())
            {
                var Name = context.Projects.FirstOrDefault(x => x.Name == EditProjectName); ;
                var found = context.Projects.FirstOrDefault(x => x.Id == ID);

                if (Name != null)
                {

                    if (found.Name == EditProjectName)
                    {
                        found.Name = EditProjectName;
                        found.Description = EditDescription;
                        found.Id = ID;
                        context.Projects.Update(found);
                        context.SaveChanges();
                        Refresh();
                    }
                    else
                    {
                        OnErrorCreated("EditProjectName", "Project already exist");
                    }

                }
                else
                {
                    found.Name = EditProjectName;
                    found.Description = EditDescription;
                    found.Id = ID;
                    context.Projects.Update(found);
                    context.SaveChanges();
                    Refresh();
                    ClearPropertyErrors(this, "EditProjectName");
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

        private int _ID;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; OnPropertyChanged(nameof(ID)); }
        }

        private string _EditprojectName;

        public string EditProjectName
        {
            get { return _EditprojectName; }
            set
            {
                _EditprojectName = value;
                OnPropertyChanged("EditProjectName");
                ClearPropertyErrors(this, "EditProjectName");

            }
        }
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

        private string _Editdescripiton;

        public string EditDescription
        {
            get { return _Editdescripiton; }
            set
            {
                _Editdescripiton = value;
                OnPropertyChanged("EditDescription");
                ClearPropertyErrors(this, "EditDescription");

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
