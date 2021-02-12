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
        bool Mode;
        public event EventHandler CloseForm;
        public event EventHandler<Func<User, bool>> LoginTry;
        public LoginViewModel(bool IsAdmin=true)
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
           
            var expression = new Func<User, bool>(u => u.Password == pb.Password && u.Login == LoginStr&&u.IsAdmin==Mode);
            User = new User { Password = pb.Password, Login = LoginStr};
            LoginTry?.Invoke(this, expression);




        }

        public void LoginCallBack(object obj,EventArgs args)
        {
            if (obj is bool val)
            {
                if (val)
                {
                    CloseForm?.Invoke(val,EventArgs.Empty);
                    return;
                }
            }
            MessageBox.Show("This admin does not exist");
        }

        private void ExecCancelCommand(object obj)
        {
            CloseForm?.Invoke(false, EventArgs.Empty);
        }
    }
}
