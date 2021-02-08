using Base_MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public event EventHandler CloseForm;
        public LoginViewModel(IEnumerable<User> Users)
        {
            this.Users = Users;
            
        }
        protected IEnumerable<User> Users { get; set; }
        private ICommand logincommand;
        private ICommand cancelCommand;
        private string loginStr;

        public string LoginStr { get => loginStr; set { loginStr = value; OnPropertyChanged("LoginStr"); } }
        public ICommand LoginCommand
        {
            get
            {
                if (logincommand == null)
                    logincommand = new RelayCommand(ExecLoginCommand, CanExecLoginCommand);
                return logincommand;
            }
        }
        public User User { get; set; }
        private bool CanExecLoginCommand(object arg)
        {
            var pb = arg as PasswordBox;
            return !string.IsNullOrWhiteSpace(LoginStr) && !string.IsNullOrWhiteSpace(pb.Password);
        }

        private void ExecLoginCommand(object obj)
        {
            var pb = obj as PasswordBox;
            var expression = new Func<User,bool>(u => u.Password == pb.Password && u.Login == LoginStr);
            if (Users.Any(expression))
            {
                var user = Users.First(expression);
                if (user != null)
                {
                    User = user;
                    CloseForm?.Invoke(true, EventArgs.Empty);
                    return;
                }
            }
            MessageBox.Show("The user is not exits");



        }

        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                    cancelCommand = new RelayCommand(ExecCancelCommand);
                return cancelCommand;
            }
        }


        private void ExecCancelCommand(object obj)
        {
            CloseForm?.Invoke(false, EventArgs.Empty);
        }
    }
}
