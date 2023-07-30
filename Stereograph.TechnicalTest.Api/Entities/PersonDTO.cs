namespace Stereograph.TechnicalTest.Api.Models;

public class PersonDTO
{
    public int? Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public PersonDTO() { }

    public PersonDTO(Person person)
    {
        this.Id = person.Id;
        this.FirstName = person.FirstName;
        this.LastName = person.LastName;
        this.Email = person.Email;
        this.Address = person.Address;
        this.City = person.City;
    }
}
