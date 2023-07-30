using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace Stereograph.TechnicalTest.Api.Controllers;

using Services;
using Models;

[Route("api/followings")]
[ApiController]
public class FollowingController : ControllerBase
{
    private FollowingService Service { get; init; }

    public FollowingController(FollowingService service)
    {
        this.Service = service;
    }

    [HttpGet]
    [Route("")]
    public IActionResult GetAll() => this.Ok(this
        .Service
        .GetAll()
        .Select(following => new FollowingDTO(following))
    );

    [HttpPost]
    [Route("")]
    public IActionResult Add([FromBody] FollowingDTO toAdd)
    {
        Following added = this.Service.Add(toAdd);
        return this.StatusCode((int)HttpStatusCode.Created);
    }

    [HttpDelete]
    [Route("")]
    public IActionResult Remove([FromQuery] int? followerId, int? followedPersonId)
    {
        if (followerId is null || followedPersonId is null)
        {
            return this.BadRequest();
        }

        bool wasRemoved = this.Service.Remove((int)followerId, (int)followedPersonId);
        return wasRemoved
            ? this.NoContent()
            : this.NotFound();
    }
}
