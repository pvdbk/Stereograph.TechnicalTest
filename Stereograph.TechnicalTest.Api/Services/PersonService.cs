using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Stereograph.TechnicalTest.Api.Services;

using Models;

public class PersonService
{
    private DbSet<Person> Persons { get; init; }
    private Func<int> SaveChanges { get; init; }

    public PersonService(ApplicationDbContext dbContext)
    {
        this.Persons = dbContext.Persons;
        this.SaveChanges = dbContext.SaveChanges;
    }

    public IEnumerable<Person> GetAll() => this.Persons;

    public Person GetById(int? id) => id is null
        ? null
        : this.Persons.FirstOrDefault(person => person.Id == id);

    public Person Add(Person toAdd)
    {
        this.Persons.Add(toAdd);
        this.SaveChanges();
        return toAdd;
    }

    public bool Remove(int id)
    {
        Person toRemove = this.GetById(id);
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
}
