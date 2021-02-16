using Client_Testing.View;
using Networking;
using Server_Designer.ViewModel;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Client_Testing
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Thread.Sleep(20000);
                ClientWrapper ClientWrapper = new ClientWrapper();

                ClientWrapper.Start("192.168.0.103", 8888);


                //Client.ConnectToServer("127.0.0.1", 8888);

                if (ClientWrapper.client.IsConnected())
                {
                    LoginUser loginUser = new LoginUser();
                    LoginViewModel loginViewModel = new LoginViewModel(false);


                    ClientWrapper.LoginAnswer += loginViewModel.LoginCallBack;

                    loginViewModel.CloseForm += loginUser.ButtonClicked;

                    loginViewModel.LoginTry += ClientWrapper.SendLoginTry;
                    //loginUser.



                    loginUser.DataContext = loginViewModel;

                    if (loginUser.ShowDialog() == true)
                    {



                        MainUserViewModel mainUserViewModel = new MainUserViewModel(loginViewModel.User,ClientWrapper);
                        MainUserForm mainUserForm = new MainUserForm();

                        mainUserForm.DataContext = mainUserViewModel;
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        Current.MainWindow = mainUserForm;
                        mainUserForm.Show();
                        mainUserForm.Closed +=mainUserViewModel.OnViewClosing; 
                        return;
                    }

                    // }


                }
                
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);

            }
            Current.Shutdown(-1);
        }

       
    }

}
