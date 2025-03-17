using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Model;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.Utils.Services
{
    public class AddressService : IAddressService
    {
        private readonly IPersonRepository _personRepository;

        public AddressService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public void Add(Person person, Address newAddress)
        {
            person.Addresses.Add(newAddress);
            _personRepository.Update(person);
        }
    }
}
