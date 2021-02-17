using Base_MVVM;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public delegate void LoginTry(User user);
    public delegate void LoginAnswer(int answerFromServer);
    public class LoginViewModel : ViewModelBase
    {
        bool Mode;
        public event EventHandler CloseForm;
        public event LoginTry LoginTry;
        public LoginViewModel(bool IsAdmin = true)
        {

            Mode = IsAdmin;
        }
        #region Commands
        private ICommand logincommand;
        private ICommand cancelCommand;
        public ICommand LoginCommand
        {
            get
            {
                if (logincommand == null)
                    logincommand = new RelayCommand(ExecLoginCommand, CanExecLoginCommand);
                return logincommand;
            }
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
        #endregion
        #region Properties
        private string loginStr;

        public string LoginStr { get => loginStr; set { loginStr = value; OnPropertyChanged("LoginStr"); } }

        public User User { get; set; }
        #endregion
        private bool CanExecLoginCommand(object arg)
        {
            var pb = arg as PasswordBox;
            return !string.IsNullOrWhiteSpace(LoginStr) && !string.IsNullOrWhiteSpace(pb.Password);
        }

        private void ExecLoginCommand(object obj)
        {
            var pb = obj as PasswordBox;

            var expression = new Func<User, bool>(u => u.Password == pb.Password && u.Login == LoginStr && u.IsAdmin == Mode);
            User = new User { Password = pb.Password, Login = LoginStr, IsAdmin = Mode };
            LoginTry?.Invoke(User);




        }

        public void LoginCallBack(int answer)
        {

            if (answer != 0)
            {
                User.Id = answer;
                CloseForm?.Invoke(true, EventArgs.Empty);
                return;
            }

            MessageBox.Show("This user does not exist");
        }

        private void ExecCancelCommand(object obj)
        {
            CloseForm?.Invoke(false, EventArgs.Empty);
        }
    }
}
