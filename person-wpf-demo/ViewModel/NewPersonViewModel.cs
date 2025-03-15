using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using person_wpf_demo.Model;
using person_wpf_demo.Model.DAL;
using person_wpf_demo.Model.Interfaces;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.ViewModel
{
    class NewPersonViewModel : BaseViewModel
    {
        private readonly IPersonDAL _personDAL;
        private readonly INavigationService _navigationService;

        private string _prenom;
        public string Prenom
        {
            get => _prenom;
            set
            {
                if (_prenom != value)
                {
                    _prenom = value;
                    OnPropertyChanged(nameof(Prenom));
                    ValidateProperty(nameof(Prenom), value);
                }
            }
        }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    OnPropertyChanged(nameof(Nom));
                    ValidateProperty(nameof(Nom), value);
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

        public NewPersonViewModel(IPersonDAL personDAL, INavigationService navigationService)
        {
            _personDAL = personDAL;
            _navigationService = navigationService;
            SaveCommand = new RelayCommand(Save, CanSave);
        }

        public ICommand SaveCommand { get; set; }
        private void Save()
        {
            Person person = new Person()
            {
                Prenom = Prenom,
                Nom = Nom,
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
            _personDAL.Save(person);
            _navigationService.NavigateTo<PersonsViewModel>();
        }
        private bool CanSave()
        {
            bool allRequiredFieldsAreEntered = Prenom.NotEmpty() && Nom.NotEmpty() && Street.NotEmpty() && City.NotEmpty() && PostalCode.NotEmpty();

            return !HasErrors && allRequiredFieldsAreEntered;
        }

        private void ValidateProperty(string propertyName, string value)
        {
            ClearErrors(propertyName);
            switch (propertyName)
            {
                case nameof(Prenom):
                    if (value.Empty())
                    {
                        AddError(propertyName, "Le prénom est requis.");
                    }
                    else if (value.Length < 2)
                    {
                        AddError(propertyName, "Le prénom doit contenir au moins 2 caractères.");
                    }
                    break;
                case nameof(Nom):
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
