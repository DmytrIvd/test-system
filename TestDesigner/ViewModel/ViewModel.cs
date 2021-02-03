using Base_MVVM;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using TestLibrary;

namespace TestDesigner
{
    class TestDesignerViewModel : ViewModelBase
    {

        public TestDesignerViewModel()
        {
            InitValues();
        }
        public TestDesignerViewModel(Test test, string path)
        {

            InitValues();
            this.test = test;
            this.path = path;
            this.title = test.Title;
            this.author = test.Author;
            foreach (var q in test.Questions)
                Questions.Add(q);
        }
        private Test test { get; set; }
        private string path { get; set; }
        private void InitValues()
        {

            Questions = new ObservableCollection<Question>();

            Variants = new ObservableCollection<Variant>();
            LevelsOfDificulty = new ObservableCollection<int>(){
                1,2,3,4,5
            };
            sfd = new SaveFileDialog();
            sfd.Filter = "XML Files (*.xml)|*.xml";
            sfd.FilterIndex = 0;
            sfd.DefaultExt = "xml";
        }
        #region Properties methods

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
        #endregion
        #region Properties
        public ObservableCollection<int> LevelsOfDificulty { get; set; }

        public ObservableCollection<Question> Questions { get; set; }

        private string author;
        public string Author
        {
            get => author;
            set { author = value; OnPropertyChanged("Author"); }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {

                title = value;
                OnPropertyChanged("Title");
            }
        }

        private Question currentQuestion;

        public Question CurrentQuestion
        {
            get => currentQuestion;
            set
            {
                if (currentQuestion != null)
                    SavePreviousProperties();

                currentQuestion = value;
                ChangeProperties(value);

                OnPropertyChanged("CurrentQuestion");
            }
        }

        public SaveFileDialog sfd { get; private set; }
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
                    deleteQuestion = new RelayCommand(ExecDeleteQuestion, CanDeleteQuestion);
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

        private ICommand addQuestion;
        public ICommand AddQuestion
        {
            get
            {
                if (addQuestion == null)
                {
                    addQuestion = new RelayCommand(AddQuestionExec, CanAddQuestion);
                }
                return addQuestion;

            }
        }


        private ICommand saveCommand;

        public ICommand Save
        {

            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(SaveTestExec);
                }
                return saveCommand;
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

        #region Command handlers
        #region Delete answer
        private bool CanDeleteAnswer(object arg)
        {

            return Variants.Count > 2;
        }
        private void DeleteAnswer_Exec(object obj)
        {

            var variant = obj as Variant;
            Variants.Remove(variant);

        }
        #endregion
        #region Add question
        private void AddQuestionExec(object obj)
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
        private bool CanAddQuestion(object arg)
        {
            return CurrentQuestion == null && IsPropertiesNotEmpty();
        }
        #endregion
        #region Add answer
        private bool CanAddNewAnswer(object arg)
        {
            return Variants.Count < 9;
        }
        private void AddNewAnswer_Exec(object obj)
        {
            Variants.Add(new Variant() { IsRight = false, Variant_str = "<empty>" }); ;
        }
        #endregion
        #region Delete question
        private bool CanDeleteQuestion(object arg)
        {
            return Questions.Count > 5;
        }
        private void ExecDeleteQuestion(object obj)
        {
            var q = obj as Question;
            Questions.Remove(q);
        }
        #endregion
        #region Save test to file
        private bool ShowFileDialog()
        {
            if (sfd.ShowDialog() == true)
            {

                if (!String.Equals(Path.GetExtension(sfd.FileName),
                       ".xml",
                       StringComparison.OrdinalIgnoreCase))
                {
                    // Invalid file type selected; display an error.
                    MessageBox.Show("The type of the selected file is not supported by this application. You must select an XML file.",
                                    "Invalid File Type"
                                   );
                    return ShowFileDialog();
                }
                return true;
            }
            return false;
        }
        private void SaveTestExec(object obj)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(author) && Questions.Count != 0)
            {
                FileMode fileMode = FileMode.Truncate;
                test = new Test()
                {
                    Author = author,
                    Title = title,
                    Questions = new List<Question>(this.Questions)
                };
                if (string.IsNullOrWhiteSpace(path))
                {

                    if (ShowFileDialog())
                    {
                        path = sfd.FileName;


                        if (!File.Exists(path))
                        {
                            fileMode = FileMode.CreateNew;
                        }
                    }
                    else
                    {
                        return;
                    }

                }
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Test));
                using (var fs = File.Open(path, fileMode))
                {
                    xmlSerializer.Serialize(fs, test);
                }
            }
            else
            {
                string message = "Fill the author and title fields first! ";
                if (Questions.Count == 0)
                {
                    message = "Add some question first!";
                }
                MessageBox.Show(message, "Empty fields", MessageBoxButton.OK);
            }
        }
        #endregion
        #endregion
      
    }
   
}
