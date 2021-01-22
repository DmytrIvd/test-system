using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TestLibrary;

namespace TestDesigner
{
    class TestDesignerViewModel : INotifyPropertyChanged
    {
        public TestDesignerViewModel()
        {
            Test = new Test();
            Test.Questions = new List<Question>(){
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
            LevelsOfDificulty = new ObservableCollection<int>(){
            1,2,3,4,5
            };
        }
        public Test Test
        {
            get => test; 
            set
            {
                test = value;
                OnPropertyChanged("Test");
            }
        }
        public Question CurrentQuestion
        {
            get => currentQuestion; 
            set
            {
                currentQuestion = value;
                OnPropertyChanged("CurrentQuestion");
            }
        }
        public ObservableCollection<int> LevelsOfDificulty{ get; set; }
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
        private Test test;
        private Question currentQuestion;

        public ICommand SaveQuestion
        {
            get
            {
                if (saveQuestion == null)
                {
                    saveQuestion = new RelayCommand(SaveQuestionExec);
                }
                return saveQuestion;

            }
        }


        #endregion
        #region CommandHandlers



        private bool CanDeleteAnswer(object arg)
        {

            return true;
        }
        private void DeleteAnswer_Exec(object obj)
        {
            throw new NotImplementedException();
        }

        private void SaveQuestionExec(object obj)
        {
            throw new NotImplementedException();
        }

        private bool CanAddNewAnswer(object arg)
        {
            return true;
        }
        private void AddNewAnswer_Exec(object obj)
        {
            throw new NotImplementedException();
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
