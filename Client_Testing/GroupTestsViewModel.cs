using Base_MVVM;
using System.Collections.ObjectModel;
using System.Windows.Input;
using TestLibrary;

namespace Client_Testing
{
    public class GroupTestsViewModel : ViewModelBase
    {
        public ObservableCollection<Group> Groups { get => groups; set { groups = value; OnPropertyChanged("Groups"); } }
        public ObservableCollection<Test> Tests { get; set; }
        public ClientWrapper Wrapper;
        public User Login { get; }

        public GroupTestsViewModel(User login, ClientWrapper clientWrapper)
        {
            Login = login;
            Wrapper = clientWrapper;
            Groups = new ObservableCollection<Group>();

        }
        public void RefreshGroups(Group[] groups)
        {
            Groups.Clear();
            foreach (var g in groups)
            {
                Groups.Add(g);
            }
        }

        public void RefreshTests(Test[] tests)
        {
            foreach (var t in tests)
            {
                Tests.Add(t);
            }
        }

        private ICommand refreshcommand;
        private ObservableCollection<Group> groups;

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
            Wrapper?.SendGroupsRequest(Login);
        }
    }

}
