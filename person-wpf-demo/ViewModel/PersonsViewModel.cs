using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                OnPropertyChanged(nameof(AddressCount));
            }
        }

        public int AddressCount => SelectedPerson?.Addresses?.Count ?? 0;

        public ICommand DeleteCommand { get; set; }
        public ICommand AddAddressCommand { get; set; }

        public PersonsViewModel(IPersonDAL personDAL, INavigationService navigationService)
        {
            _personDAL = personDAL;
            _navigationService = navigationService;
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            AddAddressCommand = new RelayCommand(AddAddress, CanAddAddress);
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

        private void AddAddress()
        {
            if (SelectedPerson != null)
            {
                var newAddress = new Address
                {
                    Street = "Nouvelle Rue",
                    City = "Nouvelle Ville",
                    PostalCode = "00000",
                    PersonId = SelectedPerson.Id
                };
                SelectedPerson.Addresses.Add(newAddress);
                _personDAL.Update(SelectedPerson);
                OnPropertyChanged(nameof(AddressCount));
            }
        }

        private bool CanAddAddress()
        {
            return SelectedPerson != null;
        }
    }
}
