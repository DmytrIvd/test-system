using Base_MVVM;
using System;
using System.Windows.Input;

namespace Server_Designer.ViewModel
{
    public abstract class EntityViewModel : ViewModelBase
    {
        // public IRepository Repository { get; set; }
        public event EventHandler SaveChangesEvent;

        private ICommand savechangesCommand;
        private RelayCommand refreshCommand;
        private ICommand addEditCommand;
        private ICommand deleteCommand;
        protected EntityViewModel()
        {

        }

        public ICommand SaveChanges
        {
            get
            {
                if (savechangesCommand == null)
                    savechangesCommand = new RelayCommand(SaveChangesExec);
                return savechangesCommand;
            }
        }

        public ICommand Refresh
        {
            get
            {
                if (refreshCommand == null)
                    refreshCommand = new RelayCommand(RefreshExec);
                return refreshCommand;
            }
        }
        public ICommand Save
        {
            get
            {
                if (addEditCommand == null)
                    addEditCommand = new RelayCommand(SaveExec, CanSaveExec);
                return addEditCommand;
            }
        }



        public ICommand Delete
        {
            get
            {
                if (deleteCommand == null)
                    deleteCommand = new RelayCommand(DeleteExec, CanDeleteExec);
                return deleteCommand;
            }
        }



        protected void SaveAll()
        {
            SaveChangesEvent?.Invoke(this, EventArgs.Empty);
        }
        #region Abstract methods
        protected abstract void DeleteExec(object obj);
        protected abstract bool CanDeleteExec(object obj);
        protected abstract void SaveExec(object obj);
        protected abstract void RefreshExec(object obj);
        protected abstract void SaveChangesExec(object obj);
        protected abstract bool CanSaveExec(object arg);
        protected abstract bool PropertiesIsNotNull();
        protected abstract void ClearProperties();
        protected abstract void ChangeProperties(object obj);
        #endregion
    }
}
// public class 

