using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using Xunit;
using Moq;

namespace Stereograph.TechnicalTest.Tests;

using Api.Services;
using Api.Models;

public class PersonServiceUnitTest
{
    [Fact]
    public void GetAllPersons()
    {
        PersonService service = new(this.GetMockContext().Object);
        Assert.Equal(3, service.GetAll().Count());
    }

    [Fact]
    public void GetUnexistingPersonById()
    {
        PersonService service = new(this.GetMockContext().Object);
        Assert.Null(service.GetById(4));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetExistingPersonById(int id)
    {
        PersonService service = new(this.GetMockContext().Object);
        Assert.Equal(id, service.GetById(id).Id);
    }

    [Fact]
    public void AddPerson()
    {
        Person toAdd = new();
        Mock<DbSet<Person>> mockSet = this.GetMockSet();
        Mock<ApplicationDbContext> mockContext = this.GetMockContext(mockSet);
        PersonService service = new(mockContext.Object);

        Assert.Equal(service.Add(toAdd), toAdd);
        mockSet.Verify(m => m.Add(toAdd), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void RemoveUnexistingPerson()
    {
        Mock<DbSet<Person>> mockSet = this.GetMockSet();
        Mock<ApplicationDbContext> mockContext = this.GetMockContext(mockSet);
        PersonService service = new(mockContext.Object);

        Assert.False(service.Remove(4));
        mockSet.Verify(m => m.Remove(It.IsAny<Person>()), Times.Never);
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void RemoveExistingPerson(int id)
    {
        List<Person> persons = this.GetNewPersons();
        Person toRemove = persons.ElementAt(id - 1);
        Mock<DbSet<Person>> mockSet = this.GetMockSet(persons);
        Mock<ApplicationDbContext> mockContext = this.GetMockContext(mockSet);
        PersonService service = new(mockContext.Object);

        Assert.True(service.Remove(id));
        mockSet.Verify(m => m.Remove(toRemove), Times.Once);
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Fact]
    public void UpdateUnexistingPerson()
    {
        Mock<ApplicationDbContext> mockContext = this.GetMockContext();
        PersonService service = new(mockContext.Object);

        Assert.Null(service.Update(new()));
        Assert.Null(service.Update(new() { Id = 4 }));
        mockContext.Verify(m => m.SaveChanges(), Times.Never);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void UpdateExistingPerson(int id)
    {
        Person newVersion = new()
        {
            Id = id,
            FirstName = "FirstName x",
            LastName = "LastName x",
            Email = "Email x",
            Address = "Address x",
            City = "City x"
        };
        List<Person> persons = this.GetNewPersons();
        Person toUpdate = persons.ElementAt(id - 1);
        Mock<DbSet<Person>> mockSet = this.GetMockSet(persons);
        Mock<ApplicationDbContext> mockContext = this.GetMockContext(mockSet);
        PersonService service = new(mockContext.Object);

        Person updated = service.Update(newVersion);
        Assert.Equal(updated.FirstName, "FirstName x");
        AssertSimilarPersons(newVersion, updated);
        AssertSimilarPersons(newVersion, service.GetById(id));
        mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    private void AssertSimilarPersons(Person expected, Person actual) => Assert.Equal(
        JsonSerializer.Serialize(expected),
        JsonSerializer.Serialize(actual)
    );

    private List<Person> GetNewPersons() => new()
    {
        new()
        {
            Id = 1,
            FirstName = "FirstName 1",
            LastName = "LastName 1",
            Email = "Email 1",
            Address = "Address 1",
            City = "City 1"
        },
        new()
        {
            Id = 2,
            FirstName = "FirstName 2",
            LastName = "LastName 2",
            Email = "Email 2",
            Address = "Address 2",
            City = "City 2"
        },
        new()
        {
            Id = 3,
            FirstName = "FirstName 3",
            LastName = "LastName 3",
            Email = "Email 3",
            Address = "Address 3",
            City = "City 3"
        }
    };

    private Mock<DbSet<Person>> GetMockSet(IEnumerable<Person> persons = null)
    {
        IQueryable<Person> queryablePersons = (persons ?? this.GetNewPersons()).AsQueryable();
        Mock<DbSet<Person>> mockSet = new();

        mockSet.As<IQueryable<Person>>().Setup(m => m.Provider).Returns(queryablePersons.Provider);
        mockSet.As<IQueryable<Person>>().Setup(m => m.Expression).Returns(queryablePersons.Expression);
        mockSet.As<IQueryable<Person>>().Setup(m => m.ElementType).Returns(queryablePersons.ElementType);
        mockSet.As<IQueryable<Person>>().Setup(m => m.GetEnumerator()).Returns(() => queryablePersons.GetEnumerator());

        return mockSet;
    }

    private Mock<ApplicationDbContext> GetMockContext(Mock<DbSet<Person>> mockSet = null)
    {
        mockSet ??= this.GetMockSet();
        Mock<ApplicationDbContext> mockContext = new();
        mockContext.Setup(c => c.Persons).Returns(mockSet.Object);
        mockContext.Setup(c => c.Entry(It.IsAny<Person>())).Returns((EntityEntry<Person>)null);
        return mockContext;
    }
}
