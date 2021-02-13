using Base_MVVM;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using TestLibrary;

namespace Client_Testing
{
    public class GroupViewModel : ViewModelBase
    {
        private bool _isExpanded;

        public GroupViewModel(Group group)
        {
            Group = group;
            Tests = new ObservableCollection<TestViewModel>();
            //Not updating tests because first we sending just class without relationship
            //foreach(var t in )
        }
        public void UpdateTests(Test[] tests)
        {
            Application.Current.Dispatcher.Invoke(
            () =>
            {
                Tests.Clear();
                foreach (var t in tests)
                {
                    Tests.Add(new TestViewModel(t));
                }
            });
        }
        public bool IsExpanded{
            get => _isExpanded;
            set{
                _isExpanded = value;
                RequestTests?.Invoke(Id);
                OnPropertyChanged("IsExpanded");
            }
        }

        public event RequestSend RequestTests;

        public Group Group { get; private set; }

        public int Id { get { return Group.Id; } }
        public string Name { get { return Group.Name; } set { Group.Name = value; OnPropertyChanged("Name"); } }
        public ObservableCollection<TestViewModel> Tests { get; set; }
    }
}
