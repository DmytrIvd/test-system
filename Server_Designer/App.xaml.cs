using Server_Designer.Model;
using Server_Designer.View;
using Server_Designer.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Server_Designer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            UnitOfWork unitOfWork = new UnitOfWork();
            LoginForm loginForm = new LoginForm();
            
            LoginViewModel loginViewModel = new LoginViewModel(unitOfWork.Users.Get());
            loginViewModel.CloseForm += loginForm.ButtonClicked;
            loginForm.DataContext = loginViewModel;
            if (loginForm.ShowDialog() == true)
            {
                ServerMain serverMain = new ServerMain();
                serverMain.Show();
            }
            unitOfWork.Dispose();
            //LoginViewModel loginViewModel = new LoginViewModel();
            base.OnStartup(e);
        }
    }
}
