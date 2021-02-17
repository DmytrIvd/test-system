using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
using TestLibrary;

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

            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.FilterIndex = 0;
            openFileDialog.DefaultExt = "xml";
        }
        private bool IsEditing = false;
        private OpenFileDialog openFileDialog;
        private void ShowTestDesigner(Test test = null)
        {
            this.Hide();
            TestDesignerViewModel testDesigner;

            if (test == null)
                testDesigner = new TestDesignerViewModel();
            else
                testDesigner = new TestDesignerViewModel(test, openFileDialog.FileName);

            CreateTestWindow createTestWindow = new CreateTestWindow();
            createTestWindow.DataContext = testDesigner;
            createTestWindow.Show();
            createTestWindow.Closed += CreateTestWindow_Closed; ;
        }

        private void CreateTestWindow_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void NewTest_Click(object sender, RoutedEventArgs e)
        {
            ShowTestDesigner();
        }


        private Test ShowFileDialog()
        {
            if (openFileDialog.ShowDialog() == true)
            {
                Test test;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Test));
                try
                {
                    using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.OpenOrCreate))
                    {

                        test = (Test)xmlSerializer.Deserialize(fs);
                        return test;
                    }

                }
                catch (Exception exception)
                {
                    MessageBox.Show("Cannot read this file! Select another one");

                    return ShowFileDialog();
                }
            }
            return null;
        }
        private void EditTest_Click(object sender, RoutedEventArgs e)
        {


            IsEditing = true;
            Test test = ShowFileDialog();
            if (test != null)
                ShowTestDesigner(test);

        }
    }
}
