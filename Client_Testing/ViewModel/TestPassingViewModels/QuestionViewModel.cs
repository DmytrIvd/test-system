using TestLibrary;
using Base_MVVM;
using System.Collections.ObjectModel;
using System.Linq;
namespace Client_Testing.ViewModel
{
    public class QuestionViewModel : ViewModelBase
    {
        public QuestionViewModel(Question question)
        {
            this.question = question;
            AnswerViewModels = new ObservableCollection<AnswerViewModel>();
            foreach (var v in question.Variants)
            {
                AnswerViewModels.Add(new AnswerViewModel(v));
            }
        }
        private Question question;
        internal bool IsRight
        {
            get
            {
                return AnswerViewModels.Any(a => a._variant.IsRight&&a.IsSelected);
            }
        }
        public int Difficulty
        {
            get => question.Dificulty;
            set
            {
                question.Dificulty = value;
                OnPropertyChanged("Difficulty");
            }
        }
        public string Question_str
        {
            get => question.Question_str;
            set
            {
                question.Question_str = value;
                OnPropertyChanged("Question_str");
            }
        }
        public ObservableCollection<AnswerViewModel> AnswerViewModels { get; set; }

    }
}

