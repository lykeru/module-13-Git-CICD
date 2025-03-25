using NUnit.Framework;
using Moq;
using person_wpf_demo.Data.Repositories.Interfaces;
using person_wpf_demo.Models;
using person_wpf_demo.Services;
using System;

namespace person_wpf_demo_tests
{
    public class PersonServiceTests
    {
        private Mock<IPersonRepository> _personRepositoryMock;
        private PersonService _personService;

        [SetUp]
        public void Setup()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _personService = new PersonService(_personRepositoryMock.Object);
        }

        [Test]
        public void Add_ValidPerson_ShouldCallSave()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };

            _personService.Add(person);

            _personRepositoryMock.Verify(repo => repo.Save(person), Times.Once);
        }

        [Test]
        public void Add_InvalidPrenom_ShouldThrowArgumentException()
        {
            var person = new Person { Prenom = "J", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Add_InvalidNom_ShouldThrowArgumentException()
        {
            var person = new Person { Prenom = "John", Nom = "D", DateDeNaissance = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void CalculateAge_ValidDate_ShouldReturnCorrectAge()
        {
            var birthDate = new DateTime(1990, 1, 1);
            var expectedAge = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-expectedAge)) expectedAge--;

            var age = _personService.CalculateAge(birthDate);

            Assert.That(age, Is.EqualTo(expectedAge));
        }

        [Test]
        public void FindAll_ShouldReturnAllPersons()
        {
            var persons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) },
                new Person { Prenom = "Jane", Nom = "Doe", DateDeNaissance = new DateTime(1992, 2, 2) }
            };
            _personRepositoryMock.Setup(repo => repo.GetAll()).Returns(persons);

            var result = _personService.FindAll();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Prenom, Is.EqualTo("John"));
            Assert.That(result.Last().Prenom, Is.EqualTo("Jane"));
        }

        [Test]
        public void Remove_ValidPerson_ShouldCallDelete()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };

            _personService.Remove(person);

            _personRepositoryMock.Verify(repo => repo.Delete(person), Times.Once);
        }
    }
}

