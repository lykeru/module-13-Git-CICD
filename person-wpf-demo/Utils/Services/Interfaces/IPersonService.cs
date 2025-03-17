using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Model;

namespace person_wpf_demo.Utils.Services.Interfaces
{
    public interface IPersonService
    {
        public void Add(Person newPerson);
        public IEnumerable<Person> FindAll();
        public void Remove(Person person);
    }
}
