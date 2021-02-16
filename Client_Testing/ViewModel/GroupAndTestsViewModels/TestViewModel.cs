using Base_MVVM;
using System;
using System.Windows.Input;
using TestLibrary;

namespace Client_Testing
{
    public delegate void ExamStart(Test test);
    public class TestViewModel : ViewModelBase
    {
        public event ExamStart ExamStarter;
        private ICommand _startTestcommand;

        public TestViewModel(Test test)
        {
            Test = test;
        }
        public ICommand StartTest
        {
            get
            {
                if (_startTestcommand == null)
                {
                    _startTestcommand = new RelayCommand(ExecStartTest);
                }
                return _startTestcommand;
            }
        }

        private void ExecStartTest(object obj)
        {
            ExamStarter?.Invoke(((TestViewModel)obj).Test);
        }

        public Test Test { get; private set; }
        #region Test properties
        public int Id { get { return Test.Id; } }
        public TimeSpan Time { get { return Test.Time; } set { Test.Time = value; OnPropertyChanged("Time"); } }
        public string Title { get { return Test.Title; } set { Test.Title = value; OnPropertyChanged("Title"); } }
        public string Author { get { return Test.Author; } set { Test.Author = value; OnPropertyChanged("Author"); } }
        #endregion
    }
}
