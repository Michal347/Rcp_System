using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace RCP_Sys.ViewModels
{
    public class RegisterViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region ICommands
        public ICommand GoBack { get; private set; }
        public ICommand Register { get; private set; }
        public ICommand GetGender { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public IUserService UserAdd;
        
        #endregion

        public event EventHandler<UserModel> RegisterSucceded;
        public event EventHandler CancelRequested;
        #region Ctors
        public RegisterViewModel()
        {
            GoBack = new RelayCommand(x => GoBackHandler());
            Register = new RelayCommand(x => RegisterUser(), CanExecuteRegister);
            GetGender = new RelayCommand(executemethod, canexecutemethod);
            OpenFileCommand = new RelayCommand(OpenFileDialog);
            UserAdd = new UserService();
            
        }

        private void OpenFileDialog(object obj)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                FileName = op.FileName;
            }
        }

        #endregion

        #region ICommand handlers
        private bool CanExecuteRegister(object obj)
        {
            bool valiData;
           
            if (string.IsNullOrWhiteSpace(Login) || Login.Length < 1 ||
                Password == null || Password.Length < 1)
                valiData = false;
            else
                valiData = true;
            return valiData;


        }

        private bool canexecutemethod(object parameter)
        {
            if (parameter != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void executemethod(object parameter)
        {
            Gender = (string)parameter;
        }

        private void GoBackHandler()
        {
            CancelRequested.Invoke(this, null);
        }

        private void RegisterUser()
        {
            ClearPropertyErrors(this, "Email");
            ClearPropertyErrors(this, "Login");
            var hash = PasswordHasher.HashPassword(Password);
            var validEmail = EmailValidation.IsValidEmail(Email);
            UserModel output;
            using (var context = new RcpDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Username == Login);

                if (user != null)
                {
                    OnErrorCreated("Login", "*Username already exist");
                    return;
                }

                else if (validEmail == false)
                {
                    OnErrorCreated("Email", "*Invalid email");
                    return;

                }

                UserAdd.Create(
                    output = new UserModel()
                    {
                        Username = Login,
                        Name = Name,
                        Password = hash,
                        Surname = Surname,
                        DateTimeJoined = DateTime.Today,
                        Email = Email,
                        IsUserAdmin = false,
                        Gender = Gender
                    });
                context.SaveChanges();

                if (FileName != null)
                {
                    context.Picture.Add(new ProfilePictureModel()
                    {

                        ImagePath = FileName,
                        ImageToByte = File.ReadAllBytes(FileName),
                        Username = Login,


                    });
                    context.SaveChanges();
                }

                else
                {
                    context.Picture.Add(new ProfilePictureModel()
                    {

                        ImagePath = null,
                        ImageToByte = null,
                        Username = Login,


                    });
                    context.SaveChanges();
                }

                RegisterSucceded.Invoke(this, output);


            }
        }
        #endregion
            #region Event rising fields

            private string login;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
                ClearPropertyErrors(this, "Login");
            }
        }

            private string passsword;

        public string Password
        {
            get { return passsword; }
            set
            {
                passsword = value;
                OnPropertyChanged("Pasword");
            }
        }

            private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

         private string surname;

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");      
            }
        }
         private string _gender="Male";

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

        private string email;


        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
                ClearPropertyErrors(this, "Email");
            }
        }

            private int _isSuccess;
        public int IsSuccess { get { return _isSuccess; } set { _isSuccess = value; } }



        private string _fileName;


        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        #endregion



    }
}
