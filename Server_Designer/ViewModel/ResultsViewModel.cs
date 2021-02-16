using Server_Designer.Model;
using System;
using System.Collections.ObjectModel;
using TestLibrary;

namespace Server_Designer.ViewModel
{
    public class ResultsViewModel : EntityViewModel
    {
        private IRepository<Result> results;


        public ResultsViewModel(EFGenericRepository<Result> results)
        {
            this.results = results;
            Results = new ObservableCollection<Result>();

        }

        public ObservableCollection<Result> Results { get; set; }

        public ObservableCollection<Group> Groups { get; set; }




        #region Properties stuff
        protected override void ChangeProperties(object obj)
        {
            throw new System.NotImplementedException();
        }

        protected override void ClearProperties()
        {
            throw new System.NotImplementedException();
        }

        protected override bool PropertiesIsNotNull()
        {
            throw new System.NotImplementedException();
        }

        #endregion
        #region Command handlers
        protected override void RefreshExec(object obj)
        {
            Results.Clear();
            foreach (var r in results.Get())
            {
                Results.Add(r);
            }
        }

        protected override bool CanDeleteExec(object obj)
        {
            return obj != null;
        }
        protected override void DeleteExec(object obj)
        {
            results.Remove(obj as Result);
        }

        protected override void SaveChangesExec(object obj)
        {
            SaveAll();
        }

        protected override void SaveExec(object obj)
        {
            throw new System.NotImplementedException();
        }
        protected override bool CanSaveExec(object arg)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
    public class ResultViewModel : Base_MVVM.ViewModelBase
    {
        private Result _result;

        public ResultViewModel(Result result)
        {
            _result = result;
            
        }
        public DateTime DateOfPass
        {
            get
            {
                return _result.dateOfPassing;
            }
            set
            {
                _result.dateOfPassing = value;
                OnPropertyChanged("DateOfPass");

            }
        }
        public string GroupName
        {
            get
            {
                return _result.Group.Name;
            }
            set
            {
                _result.Group.Name = value;
                OnPropertyChanged("GroupName");
            }
        }
        
    }
}