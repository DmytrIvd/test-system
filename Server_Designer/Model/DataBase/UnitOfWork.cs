using Server_Designer.ViewModel;
using System;
using System.Linq;
using System.Net.Sockets;
using TestLibrary;

namespace Server_Designer.Model
{
    public delegate void LoginAnswerForClient(int answer, TcpClient forWho);
    public class UnitOfWork : IDisposable
    {
        // public
        public LoginAnswerForClient VerifyClientLogin;
        public LoginAnswer VerifyLogin;
        private ExamContext examContext = new ExamContext();
        private EFGenericRepository<User> users;
        private EFGenericRepository<Group> groups;
        private EFGenericRepository<Result> results;
        private EFGenericRepository<Test> tests;
        private EFGenericRepository<Question> questions;
        private EFGenericRepository<Variant> variants;


        private bool disposedValue;

        public void Save()
        {
            examContext.SaveChanges();
        }

        public EFGenericRepository<User> Users
        {
            get
            {
                if (users == null)
                    users = new EFGenericRepository<User>(examContext);
                return users;
            }
        }

        public void Login(User user)
        {
            // var users= Users.Get(u => u.IsAdmin == user.IsAdmin && u.Login == user.Login && u.Password == user.Password);
            //  var users = Users.GetWithInclude();

            VerifyLogin?.Invoke(AnyUser(user));
        }
        private int AnyUser(User user)
        {
            User val;
            try
            {
                val = examContext.Users.First(u => u.Login == user.Login && u.Password == user.Password && u.IsAdmin == user.IsAdmin);

            }
            catch (Exception)
            {
                val = null;
            }
            if (val == null)
            {
                return 0;
            }
            return val.Id;

        }
        public void LoginClient(User user, TcpClient tcpClient)
        {

            VerifyClientLogin?.Invoke(AnyUser(user), tcpClient);
        }



        public EFGenericRepository<Group> Groups
        {
            get
            {
                if (groups == null)
                    groups = new EFGenericRepository<Group>(examContext);
                return groups;
            }
        }
        public EFGenericRepository<Result> Results
        {
            get
            {
                if (results == null)
                    results = new EFGenericRepository<Result>(examContext);
                return results;
            }
        }
        public EFGenericRepository<Test> Tests
        {
            get
            {
                if (tests == null)
                    tests = new EFGenericRepository<Test>(examContext);
                return tests;
            }
        }
        public EFGenericRepository<Question> Questions
        {
            get
            {
                if (questions == null)
                    questions = new EFGenericRepository<Question>(examContext);
                return questions;
            }
        }
        public EFGenericRepository<Variant> Variants
        {
            get
            {
                if (variants == null)
                    variants = new EFGenericRepository<Variant>(examContext);
                return variants;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                examContext.Dispose();
                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        ~UnitOfWork()
        {
            Dispose();
        }
    }
}
