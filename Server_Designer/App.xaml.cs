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
        private void ApplicationStart(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            UnitOfWork unitOfWork = new UnitOfWork();
            LoginForm loginForm = new LoginForm();

            LoginViewModel loginViewModel = new LoginViewModel();
            unitOfWork.VerifyLogin += loginViewModel.LoginCallBack;

            loginViewModel.CloseForm += loginForm.ButtonClicked;

            loginViewModel.LoginTry += unitOfWork.Login;
            
            loginForm.DataContext = loginViewModel;
            ServerMain serverMain = new ServerMain();
            if (loginForm.ShowDialog() == true)
            {
            //Start the server

                MainViewModel mainViewModel = new MainViewModel(unitOfWork,loginViewModel.User);
                serverMain.DataContext = mainViewModel;
                serverMain.Closing +=mainViewModel.OnViewClosing;
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = serverMain;
               
                serverMain.Show();
                
            }
            else{
                Current.Shutdown(-1);
            }
           
           

           
            //LoginViewModel loginViewModel = new LoginViewModel();

            //show your MainWindow
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            //ServerMain serverMain = new ServerMain();
            //serverMain.Show();
            base.OnStartup(e);
        }
       
    }
}
