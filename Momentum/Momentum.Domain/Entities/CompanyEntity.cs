namespace Momentum.Domain.Entities;

public class CompanyEntity
{
	public Guid Id { get; set; }
	public Guid? AddressId { get; set; }
	public AddressEntity? Address { get; set; } = null;
	public required string Name { get; set; }
	public required string CUI { get; set; }
	public virtual List<ProductEntity>? Products { get; set; } = [];
}
