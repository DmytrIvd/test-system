using Networking;
using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
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
            unitOfWork.Results.Create(result);
        }

        private void Server_GetTests(object entity, System.Net.Sockets.TcpClient tcpClient)
        {
            try
            {
                if (entity is int index)
                {
                    var tests = unitOfWork.Tests.GetWithInclude(
                    (t) => t.Groups.Exists(g=>g.Id == index), 
                    (t) => t.Questions.
                    Select(q => q.Variants)).
                     Select(
                        t=>new Test {
                            Id = t.Id,Author = t.Author,Title = t.Title,Time = t.Time,
                            Groups=t.Groups.Where(g=>g.Id==index).Select(g=>new Group { Id = index }).ToList(),
                            Questions=
                                t.Questions.Select(q=>
                                    new Question {
                                      Id = q.Id,Question_str =q.Question_str,Dificulty = q.Dificulty,
                        
                                        Variants=q.Variants.Select(v=>
                                                new Variant {Id=v.Id,IsRight=v.IsRight,Variant_str=v.Variant_str 
                                          }).ToList(),
                                     }).ToList(),
                         })
                    .ToArray();
                    //var tests = unitOfWork.Tests.Get(t => t.Groups.Exists(g => g.Id == index)).Select(t => new Test { Id = t.Id, Time = t.Time, Title = t.Title, Author = t.Author });
                    //foreach (var t in tests)
                    //{
                    //    t.Questions.Clear();
                    //    unitOfWork.Questions.Get()
                    //}
                    Server.SendTests(tests.ToArray(), tcpClient);
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.Message);
            }
        }

        private void Server_GetGroups(object entity, System.Net.Sockets.TcpClient tcpClient)
        {
            try
            {
                if (entity is User user)
                {
                    var groups = unitOfWork.Groups.Get(g => g.Users.Exists(u => u.Login == user.Login && u.Password == user.Password && u.IsAdmin == user.IsAdmin)).Select(g => new Group { Name = g.Name, Id = g.Id });

                    Server.SendGroup(groups.ToArray(), tcpClient);
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show($"Cannot get groups, prorably problem with user id. Exception:{exe.Message}");
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

