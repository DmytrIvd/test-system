using System;
using System.Linq;
using TestLibrary;

namespace Server_Designer.Model
{
    public class UnitOfWork : IDisposable
    {
        public EventHandler VerifyLogin;
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

        public void Login(object sender, Func<User, bool> e)
        {
       
            var user = Users.GetWithInclude(e);
            
            VerifyLogin?.Invoke(user.Count() != 0, EventArgs.Empty);
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
