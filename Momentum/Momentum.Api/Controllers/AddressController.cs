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

	[HttpPost(Name = "InsertAddress")]
	public async Task Insert(InsertAddressRequest addressModel)
	{
		_logger.LogInformation("I am in InsertAddress");

		await _addressService.Insert(addressModel.ToModel());

		_logger.LogInformation("Insert done");
	}
}

