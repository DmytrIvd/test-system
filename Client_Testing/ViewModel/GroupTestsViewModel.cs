using Base_MVVM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using TestLibrary;

namespace Client_Testing
{
    public class GroupTestsViewModel : ViewModelBase
    {
        public ObservableCollection<GroupViewModel> Groups { get; set; }
        public ClientWrapper Wrapper;
        public User Login { get; }
        public Update MainViewModel;
        public GroupTestsViewModel(User login, ClientWrapper clientWrapper)
        {
            Login = login;
            Wrapper = clientWrapper;
            Groups = new ObservableCollection<GroupViewModel>();

        }
        public void RefreshGroups(Group[] groups)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Groups.Clear();

                foreach (var g in groups)
                {
                    var gVM = new GroupViewModel(g);
                    gVM.RequestTests += SendTestsRequest;
                    Groups.Add(gVM);
                }

            });


        }
        public void LoadTests(Test[] tests)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (tests.Count() != 0)
                {
                    var ForWhatGroup = tests.First().Id;
                    var group = Groups.First(g => g.Id == ForWhatGroup);
                    if (group != null)
                    {
                        group.UpdateTests(tests);
                    }
                }
            });
        }


        private ICommand refreshcommand;

        public ICommand Refresh
        {
            get
            {
                if (refreshcommand == null)
                    refreshcommand = new RelayCommand(ExecGroupRefresh);
                return refreshcommand;
            }
        }

        private void ExecGroupRefresh(object obj)
        {
            // var val = Groups.Count;
            Wrapper?.SendGroupsRequest(Login);
        }
        public void SendTestsRequest(int index)
        {
            Wrapper.SendTestsRequest(index);
           
        }

        
    }
    public delegate void RequestSend(int index);
}
