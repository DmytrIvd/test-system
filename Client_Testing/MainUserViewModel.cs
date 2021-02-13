using Base_MVVM;
using System;
using System.Collections.ObjectModel;
using TestLibrary;

namespace Client_Testing
{
    public delegate void Update(Action action);
    public class MainUserViewModel : ViewModelBase
    {
        public ObservableCollection<object> Children { get { return _children; } }
        ObservableCollection<object> _children;
        private User user;
        public Update View;
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }
        public void UpdateTheView(Action action){
            View?.Invoke(action);
        }
        public ClientWrapper Client { get; private set; }

        public MainUserViewModel(User user,ClientWrapper wrapper)
        {
          this.user = user; ;
            Client = wrapper;

            //Client.GotGroups += Client_GotGroups;
            _children = new ObservableCollection<object>();
            GroupTestsViewModel groupTestsViewModel = new GroupTestsViewModel(User,Client);
            Subscribe(groupTestsViewModel);
            _children.Add(groupTestsViewModel);

        }

        private void Subscribe(GroupTestsViewModel groupTestsViewModel)
        {
            Client.GotGroups += groupTestsViewModel.RefreshGroups;
            Client.GotTests += groupTestsViewModel.RefreshTests;
            groupTestsViewModel.MainViewModel += UpdateTheView;
        }

        protected override void OnDispose()
        {
            Client.Dispose();
            base.OnDispose();
        }
    }

}
