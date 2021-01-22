using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewTest_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            TestDesignerViewModel testDesigner = new TestDesignerViewModel();
            CreateTestWindow createTestWindow = new CreateTestWindow();
            createTestWindow.DataContext = testDesigner;
            createTestWindow.Show();
            createTestWindow.Closing += CreateTestWindow_Closing;
        }

        private void CreateTestWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Show();
        }
    }
}
