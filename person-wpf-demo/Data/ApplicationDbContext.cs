﻿using Microsoft.EntityFrameworkCore;
using person_wpf_demo.Models;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DemoMVVM", "DemoMVVM.db");
            Directory.CreateDirectory(Path.GetDirectoryName(dbPath));
            var connectionString = $"Data Source={dbPath}";

            optionsBuilder.UseSqlite(connectionString);
        }
    }

    public DbSet<Person> Persons { get; set; }
    public DbSet<Address> Addresses { get; set; }

    public void SeedData()
    {
        if (!Persons.Any())
        {
            var person1 = new Person { Firstname = "Christopher", Lastname = "Coulombe", DateOfBirth = new DateTime(1985, 5, 23) };
            var person2 = new Person { Firstname = "Olivier", Lastname = "Tremblay", DateOfBirth = new DateTime(1990, 8, 15) };

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
