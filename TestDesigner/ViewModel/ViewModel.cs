using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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
            Questions = new ObservableCollection<Question>();
          
            Variants = new ObservableCollection<Variant>();
            LevelsOfDificulty = new ObservableCollection<int>(){
            1,2,3,4,5
            };
        }
      
       
        private void SavePreviousProperties()
        {
            if (IsPropertiesNotEmpty())
            {
                //Variants property
                CurrentQuestion.Variants.Clear();
                CurrentQuestion.Variants.AddRange(Variants.Where(x => !string.IsNullOrWhiteSpace(x.Variant_str)));
                //Dificulty property
                CurrentQuestion.Dificulty = Dificulty;
                //Name property
                CurrentQuestion.Question_str = QStr;


                OnPropertyChanged("Value");
                
            }

        }
        private void ChangeProperties(Question value)
        {


            ClearProperties();
            if (value != null)
            {
                foreach (var v in value.Variants)
                {
               
                    Variants.Add(v);
                }
                Dificulty = value.Dificulty;
                QStr = value.Question_str;
               
            }

        }
        private void ClearProperties()
        {
            Variants.Clear();
            Dificulty = 1;
            QStr = null;  
        }
        private bool IsPropertiesNotEmpty()
        {
            return !string.IsNullOrWhiteSpace(QStr) && Variants.Count >= 2 && Variants.IsVariantsHaveOneRight();
        }
        #region Properties
        public ObservableCollection<Question> Questions { get; set; }

        private Question currentQuestion;

        public Question CurrentQuestion
        {
            get => currentQuestion;
            set
            {
                if (currentQuestion != null&&!currentQuestion.Equals(value))
                    SavePreviousProperties();

                currentQuestion = value;
                ChangeProperties(value);

                OnPropertyChanged("CurrentQuestion");
            }
        }
        public ObservableCollection<int> LevelsOfDificulty { get; set; }
        #region QuestionsProperties
        public ObservableCollection<Variant> Variants { get; set; }

        private int dificulty;
        public int Dificulty { get => dificulty; set { dificulty = value; OnPropertyChanged("Dificulty"); } }

        private string qStr;
        public string QStr { get => qStr; set { qStr = value; OnPropertyChanged("QStr"); } }
        #endregion


        
        #endregion
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

        //private ICommand showQuestion;


        //public ICommand ShowQuestion
        //{
        //    get
        //    {
        //        if (showQuestion == null)
        //        {
        //            showQuestion = new RelayCommand(ExecShowQuestion);
        //        }
        //        return showQuestion;
        //    }
        //}


        //private void ExecShowQuestion(object obj)
        //{
        //    MessageBox.Show(CurrentQuestion.Dificulty + " " + CurrentQuestion.Question_str);
        //}

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
            var newQuestion = new Question()
            {
                Question_str = QStr,
                Variants = this.Variants.ToList(),
                Dificulty = this.Dificulty
            };
            Questions.Add(newQuestion);
            ClearProperties();
        }

        private bool CanSaveQuestion(object arg)
        {
            return CurrentQuestion == null && IsPropertiesNotEmpty();
        }

        private bool CanAddNewAnswer(object arg)
        {
            return Variants.Count < 9;
        }
        private void AddNewAnswer_Exec(object obj)
        {
            Variants.Add(new Variant() { IsRight = false, Variant_str = "<empty>" });
        }

       

        private void ExecDeleteQuestion(object obj)
        {
            var q = obj as Question;
            Questions.Remove(q);
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
