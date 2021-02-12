using Networking;
using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class MainViewModel : Base_MVVM.ViewModelBase, IDisposable
    {
       //public MyServer Server { get; private set; }

        private UnitOfWork unitOfWork;

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

        public MainViewModel(UnitOfWork unitOfWork, TestLibrary.User user)
        {
           // Server = myServer;
          //  Server.OnMessageReceived += Server_OnMessageSend;
            this.unitOfWork = unitOfWork;
            User = user;
            _children = new ObservableCollection<object>();

            _children.Add(new UsersViewModel(unitOfWork.Users));
            _children.Add(new GroupUsersViewModel(unitOfWork.Users, unitOfWork.Groups, unitOfWork.Tests));
            _children.Add(new TestUsersViewModel(unitOfWork.Tests, unitOfWork.Groups, unitOfWork.Questions, unitOfWork.Variants));
            Subscribe();

        }

        //private void Server_OnMessageSend(Message message)
        //{
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    switch (message.Type)
        //    {
        //        case MessageType.VerifyLoginCommand:
        //            {
        //                HandleLogin(message.data,binaryFormatter);
        //                break;
        //            }
        //    }
        //}

        //private void HandleLogin(byte[] data,BinaryFormatter binaryFormatter)
        //{
        //    using (MemoryStream ms = new MemoryStream(data))
        //    {
        //        Func<User, bool> func =  binaryFormatter.Deserialize(ms) as Func<User, bool> func;
               
        //    }
           
        //}

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
        protected override void OnDispose()
        {
            unitOfWork.Dispose();
            //Server.Dispose();
            base.OnDispose();
        }

        internal void OnViewClosing(object sender, CancelEventArgs e)
        {
            Dispose();
        }
    }
}
// public class 

