using System.Windows;

namespace Client_Testing.View
{
    /// <summary>
    /// Interaction logic for TestingWindow.xaml
    /// </summary>
    public partial class TestingWindow : Window
    {
        public TestingWindow()
        {
            InitializeComponent();
        }
        public void CloseDialog(bool result)
        {
            DialogResult = result;
        }
    }
}
