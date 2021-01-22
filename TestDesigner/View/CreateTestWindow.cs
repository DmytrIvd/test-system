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
using System.Windows.Shapes;

namespace TestDesigner
{
    /// <summary>
    /// Interaction logic for CreateTestWindow.xaml
    /// </summary>
    public partial class CreateTestWindow : Window
    {
        public CreateTestWindow()
        {
            InitializeComponent();
        }

        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key== Key.Escape){
                listBox1.UnselectAll();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
