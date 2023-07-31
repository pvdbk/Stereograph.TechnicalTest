using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Stereograph.TechnicalTest.Api.Controllers;

using Services;
using Models;

[Route("api/persons")]
[ApiController]
public class PersonController : ControllerBase
{
    private PersonService Service { get; init; }

    public PersonController(PersonService service)
    {
        this.Service = service;
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetAll() => this.Ok(this
        .Service
        .GetAll()
        .Select(person => new PersonDTO(person))
    );

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        Person person = this.Service.GetById(id);
        return person is null
            ? this.NotFound()
            : this.Ok(new PersonExtendedDTO(person));
    }

    [HttpPost]
    [Route("")]
    public IActionResult Add([FromBody] PersonDTO toAdd)
    {
        PersonDTO added = new(this.Service.Add(new(toAdd)));
        HttpRequest request = this.HttpContext.Request;
        return this.Created($"{request.Scheme}://{request.Host}{request.Path}/{added.Id}", added);
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Remove(int id)
    {
        bool wasRemoved = this.Service.Remove(id);
        return wasRemoved
            ? this.NoContent()
            : this.NotFound();
    }

    [HttpPut]
    [Route("")]
    public IActionResult Update([FromBody] PersonDTO newVersion)
    {
        if (newVersion.Id is null)
        {
            return this.BadRequest();
        }

        Person updated = this.Service.Update(new(newVersion));
        return updated is null
            ? this.NotFound()
            : this.Ok(new PersonExtendedDTO(updated));
    }

    [HttpPut]
    [Route("reset")]
    public IActionResult Clear()
    {
        this.Service.Clear();
        return this.NoContent();
    }

    [HttpPost]
    [Route("fill")]
    public IActionResult Fill()
    {
        this.Service.Fill();
        return this.NoContent();
    }
}
