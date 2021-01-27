using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public void OnPropertyValueChanged_Refresh(object sender, PropertyChangedEventArgs e)
        {
           if(e.PropertyName=="Value"){
                this.listBox1.Items.Refresh();

            }
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listBox1.UnselectAll();
            textBox1.Focus();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
           var result= MessageBox.Show("Save changes?", "Warning", MessageBoxButton.YesNoCancel);
           switch(result){
                case MessageBoxResult.Yes:{
                        (DataContext as TestDesignerViewModel).Save.Execute(null);
                        break;
                }
                case MessageBoxResult.No:{
                        break;
                }
                case MessageBoxResult.Cancel:{
                        e.Cancel = true;
                        break;
                }
           }
        }
    }
}
