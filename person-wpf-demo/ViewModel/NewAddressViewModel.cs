using person_wpf_demo.Model;
using person_wpf_demo.Model.Interfaces;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;
using System.Windows.Input;

namespace person_wpf_demo.ViewModel
{
    class NewAddressViewModel : BaseViewModel, INavigationParameterReceiver
    {
        private readonly IPersonDAL _personDAL;
        private readonly INavigationService _navigationService;
        private Person _selectedPerson;

        public NewAddressViewModel(IPersonDAL personDAL, INavigationService navigationService)
        {
            _personDAL = personDAL;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public void Initialize(params object[] parameters)
        {
            if (parameters?.Length > 0)
            {
                _selectedPerson = parameters[0] as Person;
            }
        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                if (_street != value)
                {
                    _street = value;
                    OnPropertyChanged(nameof(Street));
                    ValidateProperty(nameof(Street), value);
                }
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                if (_city != value)
                {
                    _city = value;
                    OnPropertyChanged(nameof(City));
                    ValidateProperty(nameof(City), value);
                }
            }
        }

        private string _postalCode;
        public string PostalCode
        {
            get => _postalCode;
            set
            {
                if (_postalCode != value)
                {
                    _postalCode = value;
                    OnPropertyChanged(nameof(PostalCode));
                    ValidateProperty(nameof(PostalCode), value);
                }
            }
        }

        public ICommand SaveCommand { get; set; }
        private void Save()
        {
            var newAddress = new Address
            {
                Street = Street,
                City = City,
                PostalCode = PostalCode,
                PersonId = _selectedPerson.Id
            };

            _selectedPerson.Addresses.Add(newAddress);
            _personDAL.Update(_selectedPerson);
            _navigationService.NavigateTo<PersonsViewModel>();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Street) && !string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(PostalCode);
        }

        private void ValidateProperty(string propertyName, string value)
        {
            ClearErrors(propertyName);
            if (string.IsNullOrEmpty(value))
            {
                AddError(propertyName, $"{propertyName} est requis.");
            }
            OnPropertyChanged(nameof(ErrorMessages));
        }

    }
}
