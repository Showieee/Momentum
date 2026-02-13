using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Momentum.API.Extensions;
using Momentum.API.Models;
using Momentum.Services;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    public IProductService _productService;

    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet(Name = "GetProduct")]
    public async Task<ActionResult<List<ProductModel>>> Get()
    {
        _logger.LogInformation("I am in GetProduct");

        var result = await _productService.Get();

        _logger.LogInformation("GetProduct done");
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetProductById")]
    public async Task<ActionResult<ProductModel>> GetById([FromRoute] Guid id)
    {

        _logger.LogInformation("I am in GetProductById");

        var result = await _productService.GetById(id);

        _logger.LogInformation("GetProductById done");
        return Ok(result);
    }


    [HttpPost(Name = "InsertProduct")]
    public async Task Insert([FromBody] InsertProductRequest request)
    {
        _logger.LogInformation("I am in InsertProduct");

        await _productService.Insert(request.ToModel());

        _logger.LogInformation("Insert done");
    }

    [HttpPut("{id}", Name = "UpdateProduct")]
    public async Task Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
    {
        _logger.LogInformation("I am in UpdateProduct");

        await _productService.Update(id, request.ToModel());

        _logger.LogInformation("Update done");
    }

    [HttpDelete("{id}", Name = "DeleteProduct")]
    public async Task Delete([FromRoute] Guid id)
    {
        _logger.LogInformation("I am in Delete");

        await _productService.Delete(id);

        _logger.LogInformation("Delete done");
    }
}