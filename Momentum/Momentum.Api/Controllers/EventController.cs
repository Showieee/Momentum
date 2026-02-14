using Microsoft.AspNetCore.Mvc;
using Momentum.API.Extensions;
using Momentum.API.Models;
using Momentum.Services;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventController : ControllerBase
{
    public IEventService _eventService;

    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    [HttpGet(Name = "GetEvent")]
    public async Task<ActionResult<List<EventModel>>> Get()
    {
        _logger.LogInformation("I am in GetEvent");

        var result = await _eventService.Get();

        _logger.LogInformation("GetEvent done");
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetEventById")]
    public async Task<ActionResult<EventModel>> GetById([FromRoute] Guid id)
    {

        _logger.LogInformation("I am in GetEventById");

        var result = await _eventService.GetById(id);

        _logger.LogInformation("GetEventById done");
        return Ok(result);
    }


    [HttpPost(Name = "InsertEvent")]
    public async Task Insert([FromBody] InsertEventRequest eventModel)
    {
        _logger.LogInformation("I am in InsertEvent");

        await _eventService.Insert(eventModel.ToModel());

        _logger.LogInformation("Insert done");
    }

    [HttpPut("{id}", Name = "UpdateEvent")]
    public async Task Update([FromRoute] Guid id, [FromBody] EventModel eventModel)
    {
        _logger.LogInformation("I am in UpdateEvent");

        await _eventService.Update(id, eventModel);

        _logger.LogInformation("Update done");
    }

    [HttpDelete("{id}", Name = "DeleteEvent")]
    public async Task Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("I am in Delete");

        await _eventService.Delete(id);

        _logger.LogInformation("Delete done");
    }
}

