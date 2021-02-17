using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Base_MVVM.BaseLogin
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
        public string Login
        {
            get
            {
                return (string)GetValue(LoginProperty);
            }
            set
            {
                SetValue(LoginProperty, value);
            }
        }
        public static readonly DependencyProperty LoginProperty =
          DependencyProperty.Register(
                                    "Login",
                                    typeof(string),
                                    typeof(LoginUserControl),
                                    new PropertyMetadata(null, PropertyChangedCallback)
                                     );

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            LoginUserControl userControl = ((LoginUserControl)dependencyObject);
            userControl.logintxtbox.Text = (string)args.NewValue;
        }
        public static readonly DependencyProperty LoginCommandProperty =
        DependencyProperty.Register(
        "LoginCommand",
        typeof(ICommand),
        typeof(LoginUserControl)
        );

        public ICommand CancelCommand
        {
            get
            {
                return (ICommand)GetValue(LoginCommandProperty);
            }
            set
            {
                SetValue(LoginCommandProperty, value);
            }
        }

        public static readonly DependencyProperty CancelCommandProperty =
       DependencyProperty.Register(
       "CancelCommand",
       typeof(ICommand),
       typeof(LoginUserControl)
       );

        public ICommand LoginCommand
        {
            get
            {
                return (ICommand)GetValue(CancelCommandProperty);
            }
            set
            {
                SetValue(CancelCommandProperty, value);
            }
        }
        public event RoutedEventHandler CancelClick;
        void onButtonClick(object sender, RoutedEventArgs e)
        {
            this.CancelClick?.Invoke(this, e);
        }
    }
}
