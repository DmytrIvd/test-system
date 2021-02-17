using Base_MVVM;
using Microsoft.Win32;
using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class TestUsersViewModel : EntityViewModel
    {

        #region Test properties
        private string address;
        private ICommand selectTestCommand;
        private Test test;
        private ICommand addRelationshipCommand;
        private ICommand dropRelationshipCommand;
        private string name;

        public TestUsersViewModel(IRepository<Test> testsRepo, IRepository<Group> groupsRepo, IRepository<Question> questionsRepo, IRepository<Variant> variantRepo)
        {
            TestsRepo = testsRepo;
            GroupsRepo = groupsRepo;
            QuestionsRepo = questionsRepo;
            VariantRepo = variantRepo;
            Groups = new ObservableCollection<Group>();
            Tests = new ObservableCollection<Test>();
            TestGroups = new ObservableCollection<Group>();
            RefreshExec(null);
        }

        public string Address { get => address; set { address = value; OnPropertyChanged("Address"); } }


        #endregion
        public ICommand SelectTest
        {
            get
            {
                if (selectTestCommand == null)
                    selectTestCommand = new RelayCommand(ExecSelect);
                return selectTestCommand;
            }
        }
        public ICommand AddRelationship
        {
            get
            {
                if (addRelationshipCommand == null)
                {
                    addRelationshipCommand = new RelayCommand(ExecAddRelationship, CanExecAddRelationship);
                }
                return addRelationshipCommand;
            }
        }

        private void ExecAddRelationship(object obj)
        {
            if (!Test.Groups.Exists(x => x.Id == Group.Id))
            {
                // var attachedGroup=   groupsRepo.FindById(Group.Id);
                //attachedGroup.Users.Add(User);
                Test.Groups.Add(Group);
                SaveAll();
                // User.Groups.Add(Group);
                TestsRepo.Update(t => t.Id == Test.Id, Test.Groups, GroupsRepo.Get(), "Groups");
                // groupsRepo.Update(g => g.Id == Group.Id, Group.Users, usersRepo.Get(), "Users");
                //usersRepo.Update(User);
                SaveAll();
                ChangeProperties(Test);
            }
        }
        private bool CanExecAddRelationship(object arg)
        {

            return Group != null && Test != null && !IsTestAndGroupHaveRelationships();

        }

        public ICommand DropRelationship
        {
            get
            {
                if (dropRelationshipCommand == null)
                {
                    dropRelationshipCommand = new RelayCommand(ExecDropRelationship, CanExecDropRelationship);
                }
                return dropRelationshipCommand;
            }
        }
        private bool IsTestAndGroupHaveRelationships()
        {

            //Make better
            var group = Test.Groups.FirstOrDefault(g => g.Id == Group.Id);
            var test = Group.Tests.FirstOrDefault(t => t.Id == Test.Id);
            return group != null || test != null;


        }
        private bool CanExecDropRelationship(object arg)
        {
            return Group != null && Test != null && IsTestAndGroupHaveRelationships();
        }

        private void ExecDropRelationship(object obj)
        {
            Test.Groups.RemoveAll(u => u.Id == Group.Id);
            SaveAll();
            TestsRepo.Update(t => t.Id == Test.Id, Test.Groups, GroupsRepo.Get(), "Groups");
            SaveAll();
            ChangeProperties(Test);
        }
        private Group group { get; set; }
        #region TestProperties
        private int Id { get; set; }
        public string Title
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged("Title");
            }
        }
        public ObservableCollection<Group> TestGroups { get; set; }
        #endregion
        public Test Test { get => test; set { test = value; ChangeProperties(value); OnPropertyChanged("Test"); } }
        public Group Group { get => group; set { group = value; OnPropertyChanged("Group"); } }

        public IRepository<Test> TestsRepo { get; }
        public IRepository<Group> GroupsRepo { get; }
        public IRepository<Question> QuestionsRepo { get; }
        public IRepository<Variant> VariantRepo { get; }

        public ObservableCollection<Group> Groups { get; set; }
        public ObservableCollection<Test> Tests { get; set; }

        private void ExecSelect(object obj)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "XML Files (*.xml)|*.xml";
            ofd.FilterIndex = 0;
            ofd.DefaultExt = "xml";
            if (ofd.ShowDialog() == true)
            {
                Address = ofd.FileName;
            }
        }
        #region Properties stuff
        protected override bool PropertiesIsNotNull()
        {
            return !string.IsNullOrEmpty(Title);
        }

        protected override void ClearProperties()
        {
            Id = 0;
            Title = null;
            TestGroups.Clear();
        }

        protected override void ChangeProperties(object obj)
        {
            ClearProperties();
            if (obj != null && obj is Test test)
            {
                Id = test.Id;
                Title = test.Title;
                foreach (var g in test.Groups)
                {
                    TestGroups.Add(g);
                }
            }
        }
        #endregion
        protected override void SaveChangesExec(object obj)
        {
            throw new NotImplementedException();
        }

        protected override bool CanDeleteExec(object obj)
        {
            return Id > 0;
        }

        protected override bool CanSaveExec(object arg)
        {
            return !string.IsNullOrWhiteSpace(Address);
        }



        protected override void DeleteExec(object obj)
        {
            var Test = obj as Test;

            while (Test.Questions.Count != 0)
            {
                var q = Test.Questions.First();
                while (q.Variants.Count != 0)
                {
                    var v = q.Variants.First();
                    VariantRepo.Remove(v);

                }
                QuestionsRepo.Remove(q);
            }
            TestsRepo.Remove(Test);
            SaveAll();
            RefreshExec(null);
        }

        protected override void RefreshExec(object obj)
        {
            Groups.Clear();
            Tests.Clear();
            try
            {
                var tests = TestsRepo.GetWithInclude(t => t.Questions.Select(q => q.Variants));
                foreach (var u in tests)
                {
                    Tests.Add(u);
                }
                var groups = GroupsRepo.Get();
                foreach (var g in groups)
                {
                    Groups.Add(g);
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show($"Unable to refresh {exe.Message}", "Exception", MessageBoxButton.OK);
            }
        }


        protected override void SaveExec(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Test));
            try
            {
                Test test = null;
                using (var fs = File.Open(Address, FileMode.Open))
                {
                    test = (Test)xmlSerializer.Deserialize(fs);

                }
                if (test != null)
                {
                    test.Time = TimeSpan.FromMinutes(60);
                    AddToRepos(test);
                }
            }
            catch (Exception exe)
            {
                MessageBox.Show(exe.InnerException.Message);
                return;
            }
        }

        private void AddToRepos(Test test)
        {

            TestsRepo.Create(test);
            SaveAll();

            RefreshExec(null);

        }
    }
}
