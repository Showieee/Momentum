using Microsoft.AspNetCore.Mvc;
using Momentum.API.Extensions;
using Momentum.API.Models;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    public IPersonService _personService;

    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonService personService, ILogger<PersonController> logger)
    {
        _personService = personService;
        _logger = logger;
    }

    [HttpGet(Name = "GetPerson")]
    public async Task<ActionResult<List<PersonModel>>> Get()
    {
        _logger.LogInformation("I am in GetPersons");

        var result = await _personService.Get();

        _logger.LogInformation("GetPersons done");
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetPersonById")]
    public async Task<ActionResult<PersonModel>> GetById([FromRoute] Guid id)
    {

        _logger.LogInformation("I am in GetPersonById");

        var result = await _personService.GetById(id);

        _logger.LogInformation("GetPersonById done");
        return Ok(result);
    }


    [HttpPost(Name = "InsertPerson")]
    public async Task Insert([FromBody] InsertPersonRequest personModel)
    {
        _logger.LogInformation("I am in InsertPerson");

        await _personService.Insert(personModel.ToModel());

        _logger.LogInformation("Insert done");
    }

    [HttpPut("{id}", Name = "UpdatePerson")]
    public async Task Update([FromRoute] Guid id, [FromBody] PersonModel personModel)
    {
        _logger.LogInformation("I am in UpdatePerson");

        await _personService.Update(id, personModel);

        _logger.LogInformation("Update done");
    }

    [HttpDelete("{id}", Name = "DeletePerson")]
    public async Task Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("I am in Delete");

        await _personService.Delete(id);

        _logger.LogInformation("Delete done");
    }
}

