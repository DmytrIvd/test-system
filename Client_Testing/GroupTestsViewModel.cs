using Base_MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TestLibrary;

namespace Client_Testing
{
    public class GroupTestsViewModel : ViewModelBase
    {
        public ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<Test> Tests { get; set; }
        public ClientWrapper Wrapper;
        public User Login { get; }
        public Update MainViewModel;
        public GroupTestsViewModel(User login, ClientWrapper clientWrapper)
        {
            Login = login;
            Wrapper = clientWrapper;
            Groups =new ObservableCollection<Group>();
            Tests = new ObservableCollection<Test>();

        }
        public void RefreshGroups(Group[] groups)
        {
            //Dispatcher dispatcher = Application.Current.Dispatcher;


            //    if (!dispatcher.CheckAccess())
            //    {
            //        dispatcher.BeginInvoke((Action)(() =>
            //                                            {
            //                                                // put code for the dispatched here
            //                                            }));
            //    }
            //    else
            //    {
            //        // put code for the dispatched here
            //    }
            //// MainViewModel?.Invoke(new Action(()=>
            //  {
            //      Groups.Clear();
            //      foreach (var g in groups)
            //      {
            //          Groups.Add(g);
            //      }
            //  }));
            Application.Current.Dispatcher.Invoke(() =>
            {
                Groups.Clear();

                foreach (var g in groups)
                {
                    Groups.Add(g);
                }

            });
           

        }

        public void RefreshTests(Test[] tests)
        {
            foreach (var t in tests)
            {
                Tests.Add(t);
            }
        }

        private ICommand refreshcommand;

        public ICommand Refresh
        {
            get
            {
                if (refreshcommand == null)
                    refreshcommand = new RelayCommand(ExecRefresh);
                return refreshcommand;
            }
        }

        private void ExecRefresh(object obj)
        {
            var val = Groups.Count;
            Wrapper?.SendGroupsRequest(Login);
        }
    }

}
