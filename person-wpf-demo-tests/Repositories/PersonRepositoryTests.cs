using NUnit.Framework;
using Moq;
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
        private Mock<ApplicationDbContext> _dbContextMock;
        private PersonRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            _repository = new PersonRepository(_dbContextMock.Object);
        }

        [Test]
        public void Save_ValidPerson_ShouldAddPerson()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) };
            var personsDbSetMock = new Mock<DbSet<Person>>();

            _dbContextMock.Setup(db => db.Persons).Returns(personsDbSetMock.Object);

            _repository.Save(person);

            personsDbSetMock.Verify(dbSet => dbSet.Add(It.IsAny<Person>()), Times.Once);
        }

        [Test]
        public void GetAll_ShouldReturnAllPersons()
        {
            var persons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe", DateDeNaissance = new DateTime(1990, 1, 1) },
                new Person { Prenom = "Jane", Nom = "Doe", DateDeNaissance = new DateTime(1992, 2, 2) }
            }.AsQueryable();

            var personsDbSetMock = new Mock<DbSet<Person>>();
            personsDbSetMock.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(persons.Provider);
            personsDbSetMock.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(persons.Expression);
            personsDbSetMock.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(persons.ElementType);
            personsDbSetMock.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(persons.GetEnumerator());

            _dbContextMock.Setup(db => db.Persons).Returns(personsDbSetMock.Object);

            var result = _repository.GetAll();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Prenom, Is.EqualTo("John"));
            Assert.That(result[1].Prenom, Is.EqualTo("Jane"));
        }
    }
}
