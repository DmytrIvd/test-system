using Server_Designer.Model;
using Server_Designer.View;
using Server_Designer.ViewModel;
using System.Net.Sockets;
using System.Windows;
using TestLibrary;

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

            //For receiving message that indicates is if login data valid
            unitOfWork.VerifyLogin += loginViewModel.LoginCallBack;
            //To close a form from view model
            loginViewModel.CloseForm += loginForm.ButtonClicked;
            //Send login to check if it exists
            loginViewModel.LoginTry += unitOfWork.Login;

            loginForm.DataContext = loginViewModel;

            ServerMain serverMain = new ServerMain();

            if (loginForm.ShowDialog() == true)
            {
                ServerWrapper serverWrapper = new ServerWrapper();
                serverWrapper.LogTry += unitOfWork.LoginClient;
                unitOfWork.VerifyClientLogin += serverWrapper.SendLoginAnswer;

                //Start the server
                //Starts on common ip
                //serverWrapper.Start(8888);

                
                serverWrapper.Start(8888);

                MainViewModel mainViewModel = new MainViewModel(unitOfWork, loginViewModel.User, serverWrapper);
                serverMain.DataContext = mainViewModel;
                //To dispose the view model when close view
                serverMain.Closing += mainViewModel.OnViewClosing;

                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                Current.MainWindow = serverMain;

                serverMain.Show();

            }
            else
            {
                Current.Shutdown(-1);
            }

        }

    }
    public delegate void LoginTryForClient(User user, TcpClient client);
}

