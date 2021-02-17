using System;
using System.Windows;

namespace Client_Testing.View
{
    /// <summary>
    /// Interaction logic for LoginUser.xaml
    /// </summary>
    public partial class LoginUser : Window
    {
        public LoginUser()
        {
            InitializeComponent();

        }

        internal void ButtonClicked(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                DialogResult = (bool)sender;
            });

        }
    }
}
