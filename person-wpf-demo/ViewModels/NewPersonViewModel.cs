using System.Windows.Input;
using person_wpf_demo.Models;
using person_wpf_demo.Services.Interfaces;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;

namespace person_wpf_demo.ViewModels
{
    public class NewPersonViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged(nameof(FirstName));
                    ValidateProperty(nameof(FirstName), value);
                }
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged(nameof(LastName));
                    ValidateProperty(nameof(LastName), value);
                }
            }
        }

        private DateTime? _dateOfBirth;
        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                if (_dateOfBirth != value)
                {
                    _dateOfBirth = value;
                    OnPropertyChanged(nameof(DateOfBirth));
                }
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

        public NewPersonViewModel(IPersonService personService, INavigationService navigationService)
        {
            _personService = personService;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public ICommand SaveCommand { get; set; }
        private void Save()
        {
            Person person = new Person()
            {
                Prenom = FirstName,
                Nom = LastName,
                DateDeNaissance = DateOfBirth ?? DateTime.MinValue,
                Addresses = new List<Address>
                {
                    new Address()
                    {
                        Street = Street,
                        City = City,
                        PostalCode = PostalCode
                    }
                }
            };
            _personService.Add(person);
            _navigationService.NavigateTo<PersonsViewModel>();
        }
        private bool CanSave()
        {
            bool allRequiredFieldsAreEntered = FirstName.NotEmpty() && LastName.NotEmpty() && Street.NotEmpty() && City.NotEmpty() && PostalCode.NotEmpty();

            return !HasErrors && allRequiredFieldsAreEntered;
        }

        private void ValidateProperty(string propertyName, string value)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(FirstName):
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le prénom est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(propertyName, "Le prénom doit contenir au moins 2 caractères.");
                    }
                    break;
                case nameof(LastName):
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le nom est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(propertyName, "Le nom doit contenir au moins 2 caractères.");
                    }
                    break;
                case nameof(Street):
                    if (value.Empty())
                    {
                        AddError(propertyName, "La rue est requise.");
                    }
                    break;
                case nameof(City):
                    if (value.Empty())
                    {
                        AddError(propertyName, "La ville est requise.");
                    }
                    break;
                case nameof(PostalCode):
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le code postal est requis.");
                    }
                    break;
            }
            OnPropertyChanged(nameof(ErrorMessages));
        }
    }
}
