namespace Momentum.Domain.Entities;
public class ImageEntity
{
	public Guid Id { get; set; }
	public required string Url { get; set; }
	public string? Description { get; set; }
}
