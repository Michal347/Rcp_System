using RCP_Sys.Models;
using RCP_Sys.ViewModels;
using RCP_Sys.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace RCP_Sys
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Window loginWindow;
        private Window mainWindow;
        private Window registerWindow;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            loginWindow = new LoginView();
            var lvm = new LoginViewModel();
            lvm.LoginCompleted += OnLoginCompleted;
            lvm.RegisterRequested += OnRegisterRequested;
            loginWindow.DataContext = lvm;

            loginWindow.Show();
        }

        private void OnLoginCompleted(object sender, UserModel e)
        {
            mainWindow = new MainWindow();
            mainWindow.DataContext = new MainViewModel();

            // dipose login component 
            var lvm = (loginWindow.DataContext as LoginViewModel);
            lvm.LoginCompleted -= OnLoginCompleted;
            lvm.RegisterRequested -= OnRegisterRequested;
            loginWindow.Close();

            //Display main window component
            mainWindow.Show();
        }

        private void OnRegisterRequested(object sender, string e)
        {
            //create register component
            registerWindow = new RegisterView();
            var rvm = new RegisterViewModel();
            rvm.Login = e;
            rvm.CancelRequested += OnRegisterCancelRequested;
            rvm.RegisterSucceded += OnRegisterSucceded;
            registerWindow.DataContext = rvm;

            //dipose login component
            var lvm = loginWindow.DataContext as LoginViewModel;
            lvm.LoginCompleted -= OnLoginCompleted;
            lvm.RegisterRequested -= OnRegisterRequested;
            loginWindow.Close();

            //Show register component
            registerWindow.Show();
        }

        private void OnRegisterSucceded(object sender, UserModel e)
        {
            //create login component
            loginWindow = new LoginView();
            var lvm = new LoginViewModel();
            lvm.LoginCompleted += OnLoginCompleted;
            lvm.RegisterRequested += OnRegisterRequested;
            lvm.UserLogin = e.Username;
            loginWindow.DataContext = lvm;

            //dispose register component
            var rvm = registerWindow.DataContext as RegisterViewModel;
            rvm.CancelRequested -= OnRegisterCancelRequested;
            rvm.RegisterSucceded -= OnRegisterSucceded;
            registerWindow.Close();

            //show login component
            loginWindow.Show();
        }

        private void OnRegisterCancelRequested(object sender, EventArgs e)
        {
            //create login component
            loginWindow = new LoginView();
            var lvm = new LoginViewModel();
            lvm.LoginCompleted += OnLoginCompleted;
            lvm.RegisterRequested += OnRegisterRequested;
            loginWindow.DataContext = lvm;

            //dispose register component
            var rvm = registerWindow.DataContext as RegisterViewModel;
            rvm.CancelRequested -= OnRegisterCancelRequested;
            rvm.RegisterSucceded -= OnRegisterSucceded;
            registerWindow.Close();

            //show login component
            loginWindow.Show();
        }

    }
}
