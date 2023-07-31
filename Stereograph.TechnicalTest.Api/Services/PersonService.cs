using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System;
using CsvHelper.Configuration;
using CsvHelper;

namespace Stereograph.TechnicalTest.Api.Services;

using Models;

public class PersonService
{
    private DbSet<Person> Persons { get; init; }
    private Func<int> SaveChanges { get; init; }
    private Func<Person, EntityEntry<Person>> Entry { get; init; }

    public PersonService(ApplicationDbContext dbContext)
    {
        this.Persons = dbContext.Persons;
        this.SaveChanges = dbContext.SaveChanges;
        this.Entry = dbContext.Entry<Person>;
    }

    public IEnumerable<Person> GetAll() => this.Persons;

    private Person GetByIdWithoutLoading(int? id) => id is null
        ? null
        : this
            .Persons
            .FirstOrDefault(person => person.Id == id);

    public Person GetById(int? id)
    {
        Person person = this.GetByIdWithoutLoading(id);
        if (person is null)
        {
            return null;
        }

        EntityEntry entry = this.Entry(person);
        entry?.Collection("Followers").Load();
        entry?.Collection("FollowedPersons").Load();
        return person;
    }

    public Person Add(Person toAdd)
    {
        this.Persons.Add(toAdd);
        this.SaveChanges();
        return toAdd;
    }

    public bool Remove(int id)
    {
        Person toRemove = this.GetByIdWithoutLoading(id);
        if (toRemove is null)
        {
            return false;
        }

        this.Persons.Remove(toRemove);
        this.SaveChanges();
        return true;
    }

    /* Avec cette implémentation, l'update permet de supprimer des valeurs.
     * Il aurait été simple de faire autrement, en écrivant par exemple : toUpdate.FirstName = newVersion.FirstName ?? toUpdate.FirstName;
     */
    public Person Update(Person newVersion)
    {
        Person toUpdate = this.GetById(newVersion.Id);
        if (toUpdate is null)
        {
            return null;
        }

        toUpdate.FirstName = newVersion.FirstName;
        toUpdate.LastName = newVersion.LastName;
        toUpdate.Email = newVersion.Email;
        toUpdate.Address = newVersion.Address;
        toUpdate.City = newVersion.City;
        this.SaveChanges();
        return toUpdate;
    }

    public void Clear()
    {
        this.Persons.RemoveRange(this.Persons);
        this.SaveChanges();
    }

    public void Fill()
    {
        Dictionary<string, string> propertiesMapper = new()
        {
            ["FirstName"] = "first_name",
            ["LastName"] = "last_name",
            ["Email"] = "email",
            ["Address"] = "address",
            ["City"] = "city"
        };
        CsvConfiguration config = new(CultureInfo.InvariantCulture)
        {
            NewLine = Environment.NewLine,
            HeaderValidated = null,
            MissingFieldFound = null,
            PrepareHeaderForMatch = args => propertiesMapper.GetValueOrDefault(args.Header) ?? args.Header
        };

        string personsCsvPath = Path.Combine("Ressources", "Persons.csv");
        using (StreamReader reader = new(personsCsvPath))
        using (CsvReader csv = new(reader, config))
        {
            this.Persons.AddRange(csv.GetRecords<Person>());
        }
        this.SaveChanges();
    }
}
