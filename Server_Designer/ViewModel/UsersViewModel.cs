using AutoMapper;
using Server_Designer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class UsersViewModel : EntityViewModel
    {
        private IRepository<User> usersRepo;
        private User user;
        private string login;
        private string password;
        private bool isAdmin;

       
        #region Properties changing stuff
        protected override bool PropertiesIsNotNull(){
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }
        protected override void ClearProperties()
        {
            Id = 0;
            Login = Password = null;
            IsAdmin = false;
        }
        protected override void ChangeProperties(object obj)
        {
                
                ClearProperties();
                if (obj != null&&obj is User user)
                {
                    Id = user.Id;
                    Login = user.Login;
                    Password = user.Password;
                    IsAdmin = user.IsAdmin;
                }
            
        }
        #endregion
        public User User
        {
            get => user;
            set
            {
                user = value;
                ChangeProperties(value);
                OnPropertyChanged("User");
            }
        }
        #region User properties
        private int Id { get; set; }
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;
                OnPropertyChanged("IsAdmin");
            }
        }
        #endregion
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> Admins { get; set; }


        public UsersViewModel(EFGenericRepository<User> users)
        {
            this.usersRepo = users;
            Users = new ObservableCollection<User>();
            Admins = new ObservableCollection<User>();
            RefreshExec(null);
           
        }
        #region Commands handlers
        protected override void RefreshExec(object obj)
        {
            Users.Clear();
            Admins.Clear();
            try
            {
                var users = usersRepo.Get(u => u.IsAdmin == false);
                var admins = usersRepo.Get(a => a.IsAdmin == true);
                if (users != null)
                {
                    foreach (var u in users)
                    {
                        Users.Add(u);
                    }
                }
                if (admins != null)
                {
                    foreach (var a in admins)
                    {
                        Admins.Add(a);
                    }
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

        protected override void DeleteExec(object obj)
        {
            usersRepo.Remove(obj as User);
            SaveAll();
            RefreshExec(null);
        }

        protected override void SaveExec(object obj)
        {
            if (PropertiesIsNotNull())
            {
                if (Id == 0)
                {
                    usersRepo.Create(new User { IsAdmin = IsAdmin, Login = Login, Password = Password });
                }
                else
                {

                    usersRepo.Update(new User {Id=Id ,IsAdmin = IsAdmin, Login = Login, Password = Password });
                }
                SaveAll();
                RefreshExec(null);
            }

        }

        protected override bool CanDeleteExec(object obj)
        {
            var user = obj as User;
            if (user.IsAdmin)
                return Admins.Count > 1;
            return true;
        }

        protected override bool CanSaveExec(object arg)
        {
            return PropertiesIsNotNull();
        }
        #endregion
    }
}
// public class 

