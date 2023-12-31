using System.Collections.Generic;

namespace Stereograph.TechnicalTest.Api.Models;

public class Person
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public List<Person> Followers { get; set; }
    public List<Person> FollowedPersons { get; set; }
    public List<Following> Followings { get; set; }
    public List<Following> FollowingsBy { get; set; }

    public Person() { }

    public Person(PersonDTO person)
    {
        this.Id = person.Id;
        this.FirstName = person.FirstName;
        this.LastName = person.LastName;
        this.Email = person.Email;
        this.Address = person.Address;
        this.City = person.City;
    }
}
