using Base_MVVM;
using Client_Testing.View;
using Networking;
using Server_Designer.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TestLibrary;

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




                        MainUserForm mainUserForm = new MainUserForm();
                        
                        Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                        Current.MainWindow = mainUserForm;
                        mainUserForm.Show();
                        return;
                    }

                    // }


                }
                Current.Shutdown(-1);
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);
            }
        }


    }
    public class MainUserViewModel:ViewModelBase{
        ObservableCollection<object> _children;
        private User user;

        public User User{
            get => user;
            set{
                user = value;
                OnPropertyChanged("User");
            }
        }

        public ClientWrapper Client { get; private set; }

        public MainUserViewModel(ClientWrapper wrapper){
            Client = wrapper;
        }
    }
   
}
