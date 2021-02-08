using Base_MVVM;
using Server_Designer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class GroupUsersViewModel : EntityViewModel
    {
        IRepository<User> usersRepo;
        IRepository<Group> groupsRepo;
        //Adding literally just for one method
        //:, )
        IRepository<Test> testsRepo;
        private Group group;
        private User user;
        private ICommand addRelationshipCommand;
        private ICommand dropRelationshipCommand;
        private string name;
        private RelayCommand deleteUserCommand;

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        public Group Group
        {
            get => group;
            set
            {
                group = value;
                ChangeProperties(value);
                OnPropertyChanged("Group");
            }
        }
        public User User
        {
            get => user;
            set
            {
                user = value;
                OnPropertyChanged("User");
            }
        }
        #region Group properties
        private int Id { get; set; }
        public string Name
        {
            get => name; set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public ObservableCollection<User> GroupsUsers { get; set; }
        #endregion

        #region Properties change stuff
        protected override bool PropertiesIsNotNull()
        {
            return !string.IsNullOrEmpty(Name);
        }

        protected override void ClearProperties()
        {
            Id = 0;
            Name = null;
            GroupsUsers.Clear();
        }

        protected override void ChangeProperties(object obj)
        {
            ClearProperties();
            if (obj != null && obj is Group group)
            {
                Id = group.Id;
                Name = group.Name;
                foreach (var u in group.Users)
                {
                    GroupsUsers.Add(u);
                }
            }
        }
        #endregion
        public GroupUsersViewModel(IRepository<User> users, IRepository<Group> groups,IRepository<Test> tests)
        {
            this.usersRepo = users ?? throw new ArgumentNullException(nameof(users));
            this.groupsRepo = groups ?? throw new ArgumentNullException(nameof(groups));
            this.testsRepo = tests ?? throw new ArgumentNullException(nameof(tests)); ;
            Users = new ObservableCollection<User>();
            Groups = new ObservableCollection<Group>();
            GroupsUsers = new ObservableCollection<User>();
            RefreshExec(null);
        }
        #region Command handlers
        protected override bool CanDeleteExec(object obj)
        {
            return obj != null;
        }

        protected override bool CanSaveExec(object arg)
        {
            return PropertiesIsNotNull();
        }

        protected override void DeleteExec(object obj)
        {
            var g = obj as Group;

            //Якщо ми маємо видалити цю групу
            //Отжеее
            //Так само ми маємо стерти всі "згадки про неї"

            //Delete all keys from users
            if (g.Tests != null)
                g.Tests.Clear();
            groupsRepo.Update(gr => gr.Id == Group.Id, Group.Tests, testsRepo.Get(), "Tests");
            SaveAll();

            if (g.Users != null)
                g.Users.Clear();
            groupsRepo.Update(gr => gr.Id == Group.Id, Group.Users, usersRepo.Get(), "Users");
           
            groupsRepo.Remove(g);
            SaveAll();

            RefreshExec(null);
        }

        protected override void RefreshExec(object obj)
        {
            Groups.Clear();
            Users.Clear();
            try
            {
                var users = usersRepo.Get(u => u.IsAdmin == false);
                foreach (var u in users)
                {
                    Users.Add(u);
                }
                var groups = groupsRepo.Get();
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

        protected override void SaveChangesExec(object obj)
        {
            SaveAll();
        }

        protected override void SaveExec(object obj)
        {
            if (PropertiesIsNotNull())
            {
                if (Id == 0)
                {
                    groupsRepo.Create(new Group { Name = Name });
                }
                else
                {
                    groupsRepo.Update(new Group { Id = Id, Name = Name });
                }
                SaveAll();
                RefreshExec(null);
            }
        }
        #endregion
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
        public ICommand DeleteUser
        {
            get
            {
                if (deleteUserCommand == null)
                {
                    deleteUserCommand = new RelayCommand(ExecDeleteUser, CanExecDeleteUser);
                }
                return deleteUserCommand;
            }
        }

        private bool CanExecDeleteUser(object arg)
        {
            return arg != null;
        }
        private void DropRelationshipMethod(User user){
            Group.Users.RemoveAll(u => u.Id == user.Id);
            groupsRepo.Update(g => g.Id == Group.Id, Group.Users, usersRepo.Get(), "Users");
            SaveAll();
            ChangeProperties(Group);
        }
        private void ExecDeleteUser(object obj)
        {
           if(obj is User user){
                DropRelationshipMethod(user);
            }
        }

        private bool IsUserAndGroupHaveRelationships()
        {
           
                //Make better
                var group = User.Groups.FirstOrDefault(g => g.Id == Group.Id);
                var user = Group.Users.FirstOrDefault(u => u.Id == User.Id);
                return group != null && user != null;
            
          
        }
        private bool CanExecDropRelationship(object arg)
        {
        
            return Group!=null&&User!=null&& IsUserAndGroupHaveRelationships();

        }

        private void ExecDropRelationship(object obj)
        {
            DropRelationshipMethod(User);
           
        }

        private bool CanExecAddRelationship(object arg)
        {
            return Group != null && User != null && !IsUserAndGroupHaveRelationships();
        }

        private void ExecAddRelationship(object obj)
        {
            if (!Group.Users.Exists(x=>x.Id==User.Id)) {
                // var attachedGroup=   groupsRepo.FindById(Group.Id);
                //attachedGroup.Users.Add(User);
                Group.Users.Add(User);
                // User.Groups.Add(Group);

                groupsRepo.Update(g => g.Id == Group.Id, Group.Users, usersRepo.Get(), "Users");
                //usersRepo.Update(User);
                SaveAll();
                ChangeProperties(Group);
            }
        }
    }
}
