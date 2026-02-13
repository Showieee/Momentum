using Microsoft.AspNetCore.Mvc;
using Momentum.API.Extensions;
using Momentum.API.Models;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AddressController : ControllerBase
{
    public IAddressService _addressService;

    private readonly ILogger<AddressController> _logger;

    public AddressController(IAddressService addressService, ILogger<AddressController> logger)
    {
        _addressService = addressService;
        _logger = logger;
    }

    [HttpGet(Name = "GetAddresses")]
    public async Task<ActionResult<List<AddressModel>>> Get()
    {
        _logger.LogInformation("I am in GetAddresses");

        var result = await _addressService.Get();

        _logger.LogInformation("GetAddresses done");
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetAddressById")]
    public async Task<ActionResult<AddressModel>> GetById([FromRoute] Guid id)
    {

        _logger.LogInformation("I am in GetAddressById");

        var result = await _addressService.GetById(id);

        _logger.LogInformation("GetAddressById done");
        return Ok(result);
    }


    [HttpPost(Name = "InsertAddress")]
    public async Task Insert([FromBody] InsertAddressRequest addressModel)
    {
        _logger.LogInformation("I am in InsertAddress");

        await _addressService.Insert(addressModel.ToModel());

        _logger.LogInformation("Insert done");
    }

    [HttpPut("{id}", Name = "UpdateAddress")]
    public async Task Update([FromRoute] Guid id, [FromBody] AddressModel addressModel)
    {
        _logger.LogInformation("I am in UpdateAddress");

        await _addressService.Update(id, addressModel);

        _logger.LogInformation("Update done");
    }

    [HttpDelete("{id}", Name = "DeleteAddress")]
    public async Task Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("I am in DeleteAddress");

        await _addressService.Delete(id);

        _logger.LogInformation("Delete done");
    }
}

