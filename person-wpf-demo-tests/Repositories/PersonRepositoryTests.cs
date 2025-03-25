using NUnit.Framework;
using person_wpf_demo.Data;
using person_wpf_demo.Data.Repositories;
using person_wpf_demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace person_wpf_demo_tests
{
    public class PersonRepositoryTests
    {
        private ApplicationDbContext _dbContext;
        private PersonRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _repository = new PersonRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public void Save_ValidPerson_ShouldAddPerson()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };

            _repository.Save(person);

            var savedPerson = _dbContext.Persons.FirstOrDefault(p => p.Prenom == "John" && p.Nom == "Doe");
            Assert.That(savedPerson, Is.Not.Null);
        }

        [Test]
        public void GetAll_ShouldReturnAllPersons()
        {
            var persons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) },
                new Person { Prenom = "Jane", Nom = "Doe", DateDeNaissance = new DateTime(1992, 2, 2) }
            };
            _dbContext.Persons.AddRange(persons);
            _dbContext.SaveChanges();

            var result = _repository.GetAll();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Prenom, Is.EqualTo("John"));
            Assert.That(result[1].Prenom, Is.EqualTo("Jane"));
        }

        [Test]
        public void Update_ValidPerson_ShouldUpdatePerson()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };
            _repository.Save(person);
            _dbContext.SaveChanges();

            person.Prenom = "Johnny";
            _repository.Update(person);
            _dbContext.SaveChanges();

            var updatedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(updatedPerson.Prenom, Is.EqualTo("Johnny"));
        }

        [Test]
        public void Delete_ValidPerson_ShouldRemovePerson()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };
            _repository.Save(person);
            _dbContext.SaveChanges();

            _repository.Delete(person);
            _dbContext.SaveChanges();

            var deletedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(deletedPerson, Is.Null);
        }
    }
}


