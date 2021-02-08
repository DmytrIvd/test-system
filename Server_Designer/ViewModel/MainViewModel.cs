using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Server_Designer.ViewModel
{
    public class MainViewModel : Base_MVVM.ViewModelBase,IDisposable
    {
        private UnitOfWork unitOfWork;
        ObservableCollection<object> _children;

        public MainViewModel(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _children = new ObservableCollection<object>();

            _children.Add(new UsersViewModel(unitOfWork.Users));
            _children.Add(new GroupUsersViewModel(unitOfWork.Users, unitOfWork.Groups,unitOfWork.Tests));
            Subscribe();

        }
        private void Subscribe(){
            foreach (var c in _children)
                (c as EntityViewModel).SaveChangesEvent += MainViewModel_SaveChangesEvent;
        }
        private void MainViewModel_SaveChangesEvent(object sender, EventArgs e)
        {
            if (sender != null)
                unitOfWork.Save();
        }

        public ObservableCollection<object> Children { get { return _children; } }
        protected override void OnDispose()
        {
            unitOfWork.Dispose();
            base.OnDispose();
        }

        internal void OnViewClosing(object sender, CancelEventArgs e)
        {
            Dispose();
        }
    }
}
// public class 

