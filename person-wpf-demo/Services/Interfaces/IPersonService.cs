using System;
using System.Collections.Generic;
using person_wpf_demo.Model;

namespace person_wpf_demo.Services.Interfaces
{
    public interface IPersonService
    {
        void Add(Person newPerson);
        IEnumerable<Person> FindAll();
        void Remove(Person person);
        int CalculateAge(DateTime birthDate);
    }
}
