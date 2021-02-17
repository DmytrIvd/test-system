using TestLibrary;
using Base_MVVM;

namespace Client_Testing.ViewModel
{
    public class AnswerViewModel : ViewModelBase
    {
        public AnswerViewModel(Variant variant)
        {
            _variant = variant;
        }
       internal Variant _variant;
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        public string Variant
        {
            get => _variant.Variant_str;
            set
            {
                _variant.Variant_str = value;
                OnPropertyChanged("Variant");
            }
        }
        public int Id
        {
            get
            {
                return _variant.Id;
            }
        }
    }
}

