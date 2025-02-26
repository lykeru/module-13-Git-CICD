using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using person_wpf_demo.Model;
using person_wpf_demo.Model.Interfaces;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.ViewModel
{
    class PersonsViewModel : BaseViewModel
    {
        private readonly IPersonDAL _personDAL;
        private readonly INavigationService _navigationService;
        public ObservableCollection<Person> Persons 
        {
            get => new ObservableCollection<Person>(_personDAL.GetAll());
            set; 
        }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        private int _selectedPersonIndex;
        public int SelectedPersonIndex
        {
            get { return _selectedPersonIndex; }
            set
            {
                _selectedPersonIndex = value;
            }
        }

        public ICommand DeleteCommand { get; set; }

        public PersonsViewModel(IPersonDAL personDAL, INavigationService navigationService)
        {
            _personDAL = personDAL;
            _navigationService = navigationService;
            DeleteCommand = new RelayCommand(Delete, CanDelete);
        }

        private void Delete()
        {
            _personDAL.Delete(SelectedPerson);
            OnPropertyChanged(nameof(Persons));
        }

        private bool CanDelete()
        {
            return SelectedPerson != null;
        }
    }
}
