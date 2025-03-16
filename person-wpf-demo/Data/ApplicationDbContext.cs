using Microsoft.EntityFrameworkCore;
using person_wpf_demo.Model;
using System.IO;

public class ApplicationDbContext : DbContext
{
    // Injection des options via le constructeur.
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Définition des DbSet pour vos entités.
    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }


    //Injection des données de développement.
    public void SeedData()
    {
        if (!Persons.Any())
        {
            var person1 = new Person { Prenom = "Christopher", Nom = "Coulombe" };
            var person2 = new Person { Prenom = "Olivier", Nom = "Tremblay" };

            var address1 = new Address { Street = "742 Evergreen Terrace", City = "Springfield", PostalCode = "49007", Person = person1 };
            var address2 = new Address { Street = "221B Baker Street", City = "London", PostalCode = "NW1 6XE", Person = person2 };
            var address3 = new Address { Street = "12 Grimmauld Place", City = "London", PostalCode = "WC2N 5DU", Person = person2 };

            person1.Addresses = new List<Address> { address1 };
            person2.Addresses = new List<Address> { address2, address3 };

            Persons.AddRange(person1, person2);
            Addresses.AddRange(address1, address2, address3);

            SaveChanges();
        }
    }
}
