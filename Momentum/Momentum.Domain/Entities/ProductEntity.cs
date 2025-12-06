using Momentum.Domain.Enums;

namespace Momentum.Domain.Entities;

public class ProductEntity
{
	public Guid Id { get; set; }
	public ProductType Type { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }
	public decimal Price { get; set; } = 0;
	public Guid CompanyId { get; set; }
	public virtual required CompanyEntity Company { get; set; }

}
