using Microsoft.AspNetCore.Mvc;
using Momentum.API.Extensions;
using Momentum.API.Models;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController : ControllerBase
{
    public ICompanyService _companyService;

    private readonly ILogger<CompanyController> _logger;

    public CompanyController(ICompanyService companyService, ILogger<CompanyController> logger)
    {
        _companyService = companyService;
        _logger = logger;
    }

    [HttpGet(Name = "GetCompany")]
    public async Task<ActionResult<List<CompanyModel>>> Get()
    {
        _logger.LogInformation("I am in GetCompany");

        var result = await _companyService.Get();

        _logger.LogInformation("GetCompany done");
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCompanyById")]
    public async Task<ActionResult<CompanyModel>> GetById([FromRoute] Guid id)
    {

        _logger.LogInformation("I am in GetCompanyById");

        var result = await _companyService.GetById(id);

        _logger.LogInformation("GetCompanyById done");
        return Ok(result);
    }


    [HttpPost(Name = "InsertCompany")]
    public async Task Insert([FromBody] InsertCompanyRequest companyModel)
    {
        _logger.LogInformation("I am in InsertCompany");

        await _companyService.Insert(companyModel.ToModel());

        _logger.LogInformation("Insert done");
    }

    [HttpPut("{id}", Name = "UpdateCompany")]
    public async Task Update([FromRoute] Guid id, [FromBody] CompanyModel companyModel)
    {
        _logger.LogInformation("I am in UpdateCompany");

        await _companyService.Update(id, companyModel);

        _logger.LogInformation("Update done");
    }

    [HttpDelete("{id}", Name = "DeleteCompany")]
    public async Task Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("I am in Delete");

        await _companyService.Delete(id);

        _logger.LogInformation("Delete done");
    }
}

