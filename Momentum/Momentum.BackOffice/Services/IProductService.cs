using Momentum.Shared.Models.Responses;
using Refit;

namespace Momentum.BackOffice.Services;

public interface IProductService
{
    [Get(Momentum.Shared.Routes.Products.Get)]
    Task<IApiResponse<List<GetProductResponse>>> GetProducts(CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.Products.Insert)]
    Task<IApiResponse> InsertProduct([Body] InsertProductRequest request, CancellationToken ct = default);

    [Put(Momentum.Shared.Routes.Products.Update)]
    Task<IApiResponse> UpdateProduct(Guid id, [Body] UpdateProductRequest request, CancellationToken ct = default);

    [Delete(Momentum.Shared.Routes.Products.Delete)]
    Task<IApiResponse> DeleteProduct(Guid id, CancellationToken ct = default);
}

public class InsertProductRequest
{
    public int Type { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public Guid CompanyId { get; set; }
}

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public Guid CompanyId { get; set; }
}

public class GetProductResponse
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public Guid CompanyId { get; set; }
    public CompanyResponse? Company { get; set; }
}

public class CompanyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
