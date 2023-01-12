﻿using Microsoft.EntityFrameworkCore;
using RCP_Sys.Db;
using RCP_Sys.Models;
using RCP_Sys.Repository;
using RCP_Sys.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace RCP_Sys.ViewModels
{
    public class RegisterViewModel : BaseViewModel, INotifyPropertyChanged
    {
        #region ICommands
        public ICommand GoBack { get; private set; }
        public ICommand Register { get; private set; }
        public IUserService UserAdd;
        
        #endregion

        public event EventHandler<UserModel> RegisterSucceded;
        public event EventHandler CancelRequested;
        #region Ctors
        public RegisterViewModel()
        {
            GoBack = new RelayCommand(x => GoBackHandler());
            Register = new RelayCommand(x => RegisterUser(), CanExecuteRegister);
            UserAdd = new UserService();
            
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
                    output= new UserModel() 
                    { 
                        Username = Login, 
                        Name = Name, 
                        Password = hash, 
                        Surname = Surname, 
                        DateTimeJoined = DateTime.Now, 
                        Email = Email, 
                        IsUserAdmin = false });
                context.SaveChanges();

            }
            
            RegisterSucceded.Invoke(this, output);
            

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

        #endregion
       


    }
}
