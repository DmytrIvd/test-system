using Base_MVVM;
using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class ResultsViewModel : EntityViewModel
    {
        private IRepository<Result> results;

        private IRepository<User> users;
        private IRepository<Group> groups;
        private IRepository<Test> tests;


        public ResultsViewModel(EFGenericRepository<Result> results, EFGenericRepository<User> users, EFGenericRepository<Group> groups, EFGenericRepository<Test> tests)
        {
            this.results = results;
            Results = new ObservableCollection<ResultViewModel>();
            Groups = new ObservableCollection<Group>();
            Tests = new ObservableCollection<Test>();
            Users = new ObservableCollection<User>();
            this.users = users;
            this.groups = groups;
            this.tests = tests;
            RefreshExec(null);
        }

        public ObservableCollection<ResultViewModel> Results { get; set; }

        public ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<Test> Tests { get; set; }
        public ObservableCollection<User> Users { get; set; }

        public Group SelectedGroup
        {
            get => selectedGroup;
            set
            {
                selectedGroup = value;
                OnPropertyChanged("SelectedGroup");
            }
        }
        public Test SelectedTest
        {
            get => selectedTest;
            set
            {
                selectedTest = value;
                OnPropertyChanged("SelectedTest");
            }
        }
        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }



        private Group selectedGroup;
        private Test selectedTest;
        private User selectedUser;

        private ICommand _filterCommand;
        public ICommand Filter
        {
            get
            {
                if (_filterCommand == null)
                    _filterCommand = new RelayCommand(ExecFilter);
                return _filterCommand;
            }
        }

        private void ExecFilter(object obj)
        {
            if (obj != null)
            {
                if (obj is Group group)
                {
                    FilterByGroup(group);
                }
                else if (obj is User user)
                {
                    FilterByUser(user);
                }
            }
            else
            {
                MessageBox.Show("None value is selected");
            }
        }

        private void FilterByUser(User user)
        {
            var coll = Results.Where(r => r.SenderName == user.Login).ToArray();
            Results.Clear();
            foreach (var c in coll)
            {
                Results.Add(c);
            }
        }


        private void FilterByGroup(Group group)
        {
            var coll = Results.Where(r => r.GroupName == group.Name).ToArray();
            Results.Clear();
            foreach (var c in coll)
            {
                Results.Add(c);
            }
        }

        #region Properties stuff
        protected override void ChangeProperties(object obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearProperties()
        {
            throw new System.NotImplementedException();
        }

        protected override bool PropertiesIsNotNull()
        {
            throw new System.NotImplementedException();
        }

        #endregion
        #region Command handlers
        protected override void RefreshExec(object obj)
        {
            Results.Clear();
            foreach (var r in results.Get())
            {
                var group = groups.FindById(r.GroupId);
                var user = users.FindById(r.SenderId);
                var test = tests.FindById(r.TaskId);
                r.Task = test;
                r.Sender = user;
                r.Group = group;
                Results.Add(new ResultViewModel(r));
            }


            Groups.Clear();
            foreach (var g in groups.Get())
            {
                Groups.Add(g);
            }

            Users.Clear();
            foreach (var u in users.Get())
            {
                Users.Add(u);
            }
            Tests.Clear();
            foreach (var t in tests.Get())
            {
                Tests.Add(t);
            }
        }

        protected override bool CanDeleteExec(object obj)
        {
            return obj != null;
        }
        protected override void DeleteExec(object obj)
        {
            var res = (obj as ResultViewModel).Result;

            results.Remove(new Result { Id = res.Id });
            SaveAll();
            RefreshExec(null);
        }

        protected override void SaveChangesExec(object obj)
        {
            SaveAll();
        }

        protected override void SaveExec(object obj)
        {
            throw new System.NotImplementedException();
        }
        protected override bool CanSaveExec(object arg)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public class ResultViewModel : Base_MVVM.ViewModelBase
    {
        public Result Result { get; private set; }

        public ResultViewModel(Result result)
        {
            Result = result;

        }
        public DateTime DateOfPass
        {
            get
            {
                return Result.dateOfPassing;
            }
            set
            {
                Result.dateOfPassing = value;
                OnPropertyChanged("DateOfPass");

            }
        }
        public double Grade
        {
            get
            {
                return Math.Round(Result.PercentageOfRightAnswers);
            }
            set
            {
                Result.PercentageOfRightAnswers = value;
                OnPropertyChanged("Grade");
            }
        }
        public string GroupName
        {
            get
            {
                return Result.Group.Name;
            }
            set
            {
                Result.Group.Name = value;
                OnPropertyChanged("GroupName");
            }
        }
        public string TestTitle
        {
            get
            {
                return Result.Task.Title;
            }
            set
            {
                Result.Task.Title = value;
                OnPropertyChanged("TestTitle");
            }
        }
        public string SenderName
        {
            get
            {
                return Result.Sender.Login;
            }
            set
            {
                Result.Sender.Login = value;
                OnPropertyChanged("SenderName");
            }
        }
    }
}