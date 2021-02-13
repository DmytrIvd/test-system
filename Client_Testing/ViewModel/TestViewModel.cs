using Base_MVVM;
using System;
using TestLibrary;

namespace Client_Testing
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel(Test test)
        {
            Test = test;
        }
        public Test Test { get; private set; }

        public int Id { get { return Test.Id; } }
        public TimeSpan Time { get { return Test.Time; } set { Test.Time = value; OnPropertyChanged("Time"); } }
        public string Title { get { return Test.Title; } set { Test.Title = value; OnPropertyChanged("Title"); } }
        public string Author { get { return Test.Author; } set { Test.Author = value; OnPropertyChanged("Author"); } }

    }
}
