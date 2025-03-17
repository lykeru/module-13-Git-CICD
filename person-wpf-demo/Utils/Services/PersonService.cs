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
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }
        public void Add(Person newPerson)
        {
            _personRepository.Save(newPerson);
        }

        public IEnumerable<Person> FindAll()
        {
            return _personRepository.GetAll();
        }

        public void Remove(Person person)
        {
            _personRepository.Delete(person);
        }
    }
}
