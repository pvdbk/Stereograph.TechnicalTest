using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll() => this.Ok(this.Service.GetAll());

    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        Person person = this.Service.GetById(id);
        return person is null
            ? this.NotFound()
            : this.Ok(person);
    }

    [HttpPost]
    [Route("")]
    public IActionResult Add([FromBody] Person toAdd)
    {
        Person added = this.Service.Add(toAdd);
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
    public IActionResult Update([FromBody] Person newVersion)
    {
        if (newVersion.Id is null)
        {
            return this.BadRequest();
        }

        Person updated = this.Service.Update(newVersion);
        return updated is null
            ? this.NotFound()
            : this.Ok(updated);
    }
}
