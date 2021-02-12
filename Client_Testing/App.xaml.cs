using Client_Testing.View;
using Networking;
using Server_Designer.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Client Client = new Client();
            Client.ConnectToServer("127.0.0.1", 8888);

            if (Client.IsConnected())
            {
                LoginUser loginUser = new LoginUser();
                LoginViewModel loginViewModel = new LoginViewModel(false);


                loginViewModel.CloseForm += loginUser.ButtonClicked;
                loginUser.DataContext = loginViewModel;

                if (loginUser.ShowDialog() == true)
                {

                }

                // }
                Current.Shutdown(-1);

            }
        }


    }
}
   