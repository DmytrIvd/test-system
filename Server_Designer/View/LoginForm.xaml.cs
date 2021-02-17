using System;
using System.Windows;

namespace Server_Designer.View
{
    /// <summary>
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();

        }



        public void ButtonClicked(object sender, EventArgs e)
        {
            DialogResult = (bool)sender;
        }
    }
}
