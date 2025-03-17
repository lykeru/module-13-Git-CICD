using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Model;

namespace person_wpf_demo.Data.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _context;

        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Person> GetAll()
        {
            return _context.Persons.Include(p => p.Addresses).ToList();
        }

        public void Save(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
        }

        public void Update(Person person)
        {
            var existingPerson = _context.Persons.Include(p => p.Addresses).FirstOrDefault(p => p.Id == person.Id);
            if (existingPerson != null)
            {
                existingPerson.Prenom = person.Prenom;
                existingPerson.Nom = person.Nom;

                foreach (var address in person.Addresses)
                {
                    var existingAddress = existingPerson.Addresses.FirstOrDefault(a => a.Id == address.Id);
                    if (existingAddress != null)
                    {
                        existingAddress.Street = address.Street;
                        existingAddress.City = address.City;
                        existingAddress.PostalCode = address.PostalCode;
                    }
                    else
                    {
                        existingPerson.Addresses.Add(address);
                    }
                }

                foreach (var address in existingPerson.Addresses.ToList())
                {
                    if (!person.Addresses.Any(a => a.Id == address.Id))
                    {
                        _context.Addresses.Remove(address);
                    }
                }

                _context.Persons.Update(existingPerson);
                _context.SaveChanges();
            }
        }

        public void Delete(Person person)
        {
            _context.Persons.Remove(person);
            _context.SaveChanges();
        }
    }
}
