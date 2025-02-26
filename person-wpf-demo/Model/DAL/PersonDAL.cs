using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Model.Interfaces;

namespace person_wpf_demo.Model.DAL
{
    public class PersonDAL : IPersonDAL
    {
        private readonly List<Person> _persons;
        public PersonDAL()
        {
            _persons = [];
        }

        public IList<Person> GetAll()
        {
            return _persons;
        }

        public void Save(Person person)
        {
            _persons.Add(person);
        }

        public void Delete(Person person)
        {
            _persons.Remove(person);
        }
    }
}
