using Networking;
using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class MainViewModel : Base_MVVM.ViewModelBase, IDisposable
    {
        public ServerWrapper Server { get; private set; }

        //public MyServer Server { get; private set; }

        private UnitOfWork unitOfWork;
        /// <summary>
        /// Logined User
        /// </summary>
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }

        ObservableCollection<object> _children;
        private User user;

        public MainViewModel(UnitOfWork unitOfWork, TestLibrary.User user, ServerWrapper serverWrapper)
        {
            // Server = myServer;
            //  Server.OnMessageReceived += Server_OnMessageSend;
            Server = serverWrapper;

            Server.GetGroups += Server_GetGroups;
            Server.GetTests += Server_GetTests;
            Server.ResultSend += Server_ResultSend;

            this.unitOfWork = unitOfWork;
            User = user;
            _children = new ObservableCollection<object>();

            //User-Admins redactor
            _children.Add(new UsersViewModel(unitOfWork.Users));
            //Users-Groups-Tests redactor
            _children.Add(new GroupUsersViewModel(unitOfWork.Users, unitOfWork.Groups, unitOfWork.Tests));
            //Tests-Groups-Questions-Variants redactor
            _children.Add(new TestUsersViewModel(unitOfWork.Tests, unitOfWork.Groups, unitOfWork.Questions, unitOfWork.Variants));
            Subscribe();

        }

        private void Server_ResultSend(Result result, System.Net.Sockets.TcpClient tcpClient)
        {
            throw new NotImplementedException();
        }

        private void Server_GetTests(object entity, System.Net.Sockets.TcpClient tcpClient)
        {
            if (entity is int index)
            {
              //var tests=  unitOfWork.Tests.GetWithInclude(t => t.Id == index);
              //tests.
            }
        }

        private void Server_GetGroups(object entity, System.Net.Sockets.TcpClient tcpClient)
        {
            if (entity is User user)
            {
                var groups = unitOfWork.Groups.GetWithInclude(x => x.Users.Exists(u => u.Login == user.Login && u.Password == user.Password && u.IsAdmin == user.IsAdmin)).Select(g=>new Group {Name= g.Name,Id=g.Id });
                
                Server.SendGroup(groups.ToArray(), tcpClient);
            }
        }

        private void Subscribe()
        {
            foreach (var c in _children)
                (c as EntityViewModel).SaveChangesEvent += MainViewModel_SaveChangesEvent;
        }
        private void MainViewModel_SaveChangesEvent(object sender, EventArgs e)
        {
            if (sender != null)
                unitOfWork.Save();
        }

        public ObservableCollection<object> Children { get { return _children; } }
        #region Dispose / closing methods
        protected override void OnDispose()
        {
            unitOfWork.Dispose();
            Server.Dispose();
            //Server.Dispose();
            base.OnDispose();
        }
        internal void OnViewClosing(object sender, CancelEventArgs e)
        {
            Dispose();
        }
        #endregion
    }
}
// public class 

