using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TestLibrary;
using Base_MVVM;
using System.Collections.ObjectModel;

namespace Client_Testing.ViewModel
{

    public class TestingViewModel : Base_MVVM.ViewModelBase
    {
        #region Entities place
        public Result Result;
        public Test test { get; private set; }
        public Group group { get; private set; }
        public User sender { get; private set; }
        #endregion
        #region Timer properties
        DispatcherTimer _refreshTimer;
        DispatcherTimer ExamTimer;

        string _timeRemaining;
        public string Timer
        {
            get
            {
                return this._timeRemaining;
            }
            set
            {
                if (_timeRemaining == value)
                    return;
                _timeRemaining = value;
                OnPropertyChanged("Timer");
            }
        }

        DateTime StartedTime;

        #endregion
        public string Title
        {
            get => "Title:" + test.Title + " Author:" + test.Author;
        }
        #region Commands
        private ICommand _nextCommand;
        public ICommand Next
        {
            get
            {
                if (_nextCommand == null)
                    _nextCommand = new RelayCommand(ExecNext, CanExecNext);
                return _nextCommand;
            }
        }

        private ICommand _previousCommand;
        public ICommand Previous
        {
            get
            {
                if (_previousCommand == null)
                    _previousCommand = new RelayCommand(ExecPrevious, CanExecPrevious);
                return _previousCommand;
            }
        }

        private ICommand _endExamCommand;
        public ICommand EndExam
        {
            get
            {
                if (_endExamCommand == null)
                    _endExamCommand = new RelayCommand(ExecEndExam);
                return _endExamCommand;
            }
        }
        #endregion
        #region Command handlers
        private bool CanExecPrevious(object arg)
        {
            return Index == 0;
        }

        private void ExecPrevious(object obj)
        {
            Index--;
        }

        private bool CanExecNext(object arg)
        {
            return Index <= Questions.Count - 1;
        }

        private void ExecNext(object obj)
        {
            Index++;
        }

        private void ExecEndExam(object obj)
        {
            if (AllCount != AnsweredCount)
            {
                MessageBox.Show("Are you sure you answer all questions? You have skipped some, please answer to they first");
                var question = Questions.First(q => q.AnswerViewModels.First(a => a.IsSelected) != null);
                if (question != null)
                {
                    Index = Questions.IndexOf(question);
                    return;
                }
            }
            ResumeExam();

        }
        #endregion

        #region Test passing properties
        public ObservableCollection<QuestionViewModel> Questions = new ObservableCollection<QuestionViewModel>();

        public int AnsweredCount
        {
            get => Questions.Select(x => x.AnswerViewModels.Where(a => a.IsSelected == true)).Count();

        }

        public int AllCount
        {
            get
            {
                return Questions.Count;
            }

        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                OnPropertyChanged("Index");
                OnPropertyChanged("Difficulty");
                OnPropertyChanged("Question");
                OnPropertyChanged("Answers");
                OnPropertyChanged("AnsweredCount");
            }
        }

        public ObservableCollection<AnswerViewModel> Answers
        {
            get
            {
                return Questions[Index].AnswerViewModels;
            }
        }
        public int Difficulty
        {
            get
            {
                return Questions[Index].Difficulty;
            }

        }
        public string Question
        {
            get
            {
                return Questions[Index].Question_str;
            }

        }
        #endregion
        public TestingViewModel(Test test, User user, Group group)
        {
            Result = new Result();
            this.group = group;
            this.sender = user;
            this.test = test;
            InitElements();
        }
        protected void InitElements()
        {
            #region Timer

            ExamTimer = new DispatcherTimer(DispatcherPriority.Normal);
            ExamTimer.Interval = test.Time;
            ExamTimer.Tick += RealTimer_Tick;
            StartedTime = DateTime.Now;
            ExamTimer.Start();

            _refreshTimer = new DispatcherTimer(DispatcherPriority.Render);
            _refreshTimer.Interval = TimeSpan.FromSeconds(1);
            _refreshTimer.Tick += (sender, args) =>
              {
                  var result = StartedTime + ExamTimer.Interval - DateTime.Now;
                  Timer = string.Format("{0:00}:{1:00}:{2:00}", result.Hours, result.Minutes, result.Seconds);
              };
            #endregion
            Questions = new ObservableCollection<QuestionViewModel>();
            foreach (var q in test.Questions)
            {
                Questions.Add(new QuestionViewModel(q));
            }
        }

        public event CloseSomething CloseView;
        private void ResumeExam()
        {
            Result.dateOfPassing = DateTime.Now;
            Result.Group = group;
            Result.Sender = sender;
            Result.Task = test;
            Result.PercentageOfRightAnswers = (double)(Questions.Select(q=>q.IsRight).Count()*100)/(double)Questions.Select(q => q.Difficulty).Sum();
            MessageBox.Show("Your result is:" + Result.PercentageOfRightAnswers);
            CloseView.Invoke();
         }
        #region Event handlers
        private void RealTimer_Tick(object sender, EventArgs e)
        {
            MessageBox.Show($"Your time is over mister {Result.Sender.Login}, we closing your exam and sending results.");
            ResumeExam();
            // CloseView?.Invoke();

        }
        #endregion

    }
    public delegate void CloseSomething();
}

