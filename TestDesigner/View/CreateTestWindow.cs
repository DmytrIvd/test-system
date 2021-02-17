using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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
            if (e.PropertyName == "Value")
            {
                this.listBox1.Items.Refresh();

            }
        }
        private void ListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
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
            var result = MessageBox.Show("Save changes?", "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    {
                        (DataContext as TestDesignerViewModel).Save.Execute(null);
                        break;
                    }
                case MessageBoxResult.No:
                    {
                        break;
                    }
                case MessageBoxResult.Cancel:
                    {
                        e.Cancel = true;
                        break;
                    }
            }
        }
    }
}
