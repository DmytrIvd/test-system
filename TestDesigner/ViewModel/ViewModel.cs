using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices; 
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TestLibrary;

namespace TestDesigner
{
    class TestDesignerViewModel : INotifyPropertyChanged
    {
        public TestDesignerViewModel()
        {
            //Test = new Test();
            // Test.Questions 
            Questions = new ObservableCollection<Question>(){
            new Question()
            {
                Dificulty = 2,
                Question_str = "123",
                Variants = new List<Variant>(){
                new Variant(){
                IsRight=true,
                Variant_str="123"
                },
                new Variant(){
                IsRight=false,
                Variant_str="123"
                },
                }
            },
              new Question()
              {
                  Dificulty = 3,
                  Question_str = "222",
                  Variants = new List<Variant>(){
                new Variant(){
                IsRight=true,
                Variant_str="222"
                },
                new Variant(){
                IsRight=false,
                Variant_str="222"
                },
                }
              } };
            Variants = new ObservableCollection<Variant>();
            LevelsOfDificulty = new ObservableCollection<int>(){
            1,2,3,4,5
            };
        }
        public ObservableCollection<Question> Questions { get; set; }

        private Question currentQuestion;

        public Question CurrentQuestion
        {
            get => currentQuestion;
            set
            {
                if (currentQuestion != null)
                    SavePreviousVariants();
                currentQuestion = value;
                ChangeVariants(value);
                OnPropertyChanged("CurrentQuestion");
            }
        }
        private void SavePreviousVariants()
        {
            CurrentQuestion.Variants.Clear();
            CurrentQuestion.Variants.AddRange(Variants);
        }
        private void ChangeVariants(Question value)
        {

            //Adds new
            Variants.Clear();
            if (value != null)
            {
                foreach (var v in value.Variants)
                {
                    Variants.Add(v);
                }
            }
        }

        public ObservableCollection<Variant> Variants { get; set; }
        public ObservableCollection<int> LevelsOfDificulty { get; set; }
        #region Commands



        private ICommand deleteAnswer;
        public ICommand DeleteAnswer
        {
            get
            {
                if (deleteAnswer == null)
                {
                    deleteAnswer = new RelayCommand(DeleteAnswer_Exec, CanDeleteAnswer);
                }
                return deleteAnswer;

            }
        }

        private ICommand addQuestion;
        public ICommand AddQuestion
        {
            get
            {
                if (addQuestion == null)
                {
                    addQuestion = new RelayCommand(ExecAddQuestion, CanAddQuestion);
                }
                return addQuestion;
            }
        }

        private ICommand deleteQuestion;
        public ICommand DeleteQuestion
        {
            get
            {
                if (deleteQuestion == null)
                {
                    deleteQuestion = new RelayCommand(ExecDeleteQuestion);
                }
                return deleteQuestion;
            }
        }

        private ICommand addNewAnswer;
        public ICommand AddNewAnswer
        {
            get
            {
                if (addNewAnswer == null)
                {
                    addNewAnswer = new RelayCommand(AddNewAnswer_Exec, CanAddNewAnswer);
                }
                return addNewAnswer;
            }
        }

        private ICommand saveQuestion;
        public ICommand SaveQuestion
        {
            get
            {
                if (saveQuestion == null)
                {
                    saveQuestion = new RelayCommand(SaveQuestionExec, CanSaveQuestion);
                }
                return saveQuestion;

            }
        }
       
        private ICommand showQuestion;
        public ICommand ShowQuestion
        {
            get
            {
                if (showQuestion == null)
                {
                    showQuestion = new RelayCommand(ExecShowQuestion);
                }
                return showQuestion;
            }
        }

        private void ExecShowQuestion(object obj)
        {
            MessageBox.Show(CurrentQuestion.Dificulty + " " + CurrentQuestion.Question_str);
        }

        #endregion

        #region CommandHandlers

        private bool CanDeleteAnswer(object arg)
        {

            return Variants.Count > 2;
        }
        private void DeleteAnswer_Exec(object obj)
        {

            var variant = obj as Variant;
            Variants.Remove(variant);

        }

        private void SaveQuestionExec(object obj)
        {
            throw new NotImplementedException();
        }
        private bool CanSaveQuestion(object arg)
        {
            return CurrentQuestion == null;
        }

        private bool CanAddNewAnswer(object arg)
        {
            return Variants.Count < 9;
        }
        private void AddNewAnswer_Exec(object obj)
        {
            Variants.Add(new Variant() { IsRight = false, Variant_str = "" });
        }

        private bool CanAddQuestion(object arg)
        {
            throw new NotImplementedException();
        }
        private void ExecAddQuestion(object obj)
        {
            throw new NotImplementedException();
        }

        private void ExecDeleteQuestion(object obj)
        {
            Questions.Remove(CurrentQuestion);
        }

        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
