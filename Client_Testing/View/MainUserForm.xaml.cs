using System;
using System.Windows;

namespace Client_Testing.View
{
    /// <summary>
    /// Interaction logic for MainUserForm.xaml
    /// </summary>
    public partial class MainUserForm : Window
    {
        public void UpdateAThread(Action action)
        {
            this.Dispatcher.Invoke(action);
        }
        public MainUserForm()
        {
            InitializeComponent();
        }
    }
}
