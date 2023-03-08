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

        public ICommand GoBack { get; private set; }
        public ICommand Register { get; private set; }
        public ICommand GetGender { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public IUserService UserAdd;


        public event EventHandler<UserModel> RegisterSucceded;
        public event EventHandler CancelRequested;


        public RegisterViewModel()
        {
            GoBack = new RelayCommand(x => GoBackHandler());
            Register = new RelayCommand(x => RegisterUser(), CanExecuteRegister);
            GetGender = new RelayCommand(executemethod, canexecutemethod);
            OpenFileCommand = new RelayCommand(OpenFileDialog);
            UserAdd = new UserService();
            
        }



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
            ClearPropertyErrors(this, nameof(Email));
            ClearPropertyErrors(this, nameof(Login));

            var hash = PasswordHasher.HashPassword(Password);
            var validEmail = EmailValidation.IsValidEmail(Email);
            UserModel output;
            using (var context = new RcpDbContext())
            {
                var user = context.Users.FirstOrDefault(x => x.Username == Login);

                if (user != null)
                {
                    OnErrorCreated(nameof(Login), "*Username already exist");
                    return;
                }

                else if (validEmail == false)
                {
                    OnErrorCreated(nameof(Email), "*Invalid email");
                    return;

                }

                else if (FileName == null)
                {

                    UserAdd.Create(output= new UserModel()
                    {

                        ImagePath = null,
                        ImageToByte = null,
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
                    context.Picture.Add(new ProfilePictureModel()
                    {

                        ImagePath = null,
                        ImageToByte = null,
                        Username = Login,


                    });
                    context.SaveChanges();
                    RegisterSucceded.Invoke(this, output);
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
                      Gender = Gender,
                      ImagePath = FileName,
                      ImageToByte = File.ReadAllBytes(FileName),

                  });
                        context.SaveChanges();
                        context.Picture.Add(new ProfilePictureModel()
                        {

                            ImagePath = FileName,
                            ImageToByte = File.ReadAllBytes(FileName),
                            Username = Login,


                        });
                        context.SaveChanges();
     
                    RegisterSucceded.Invoke(this, output);

            }
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
 

        private string login;

        public string Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged(nameof(Login));
                ClearPropertyErrors(this, nameof(Login));
            }
        }

            private string passsword;

        public string Password
        {
            get { return passsword; }
            set
            {
                passsword = value;
                OnPropertyChanged(nameof(Password));
            }
        }

            private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

         private string surname;

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged(nameof(Surname));      
            }
        }
         private string _gender="Male";

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string email;


        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
                ClearPropertyErrors(this, nameof(Email));
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
                OnPropertyChanged(nameof(FileName));
            }
        }


    }
}
