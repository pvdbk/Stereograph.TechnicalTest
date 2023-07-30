using System.Collections.Generic;
using System.Linq;

namespace Stereograph.TechnicalTest.Api.Models;

public class PersonExtendedDTO
{
    public PersonDTO Person { get; set; }
    public IEnumerable<PersonDTO> Followers { get; set; }
    public IEnumerable<PersonDTO> FollowedPersons { get; set; }

    public PersonExtendedDTO(Person person)
    {
        this.Person = new PersonDTO(person);
        this.Followers = person.Followers.Select(follower => new PersonDTO(follower));
        this.FollowedPersons = person.FollowedPersons.Select(followedPerson => new PersonDTO(followedPerson));
    }
}
