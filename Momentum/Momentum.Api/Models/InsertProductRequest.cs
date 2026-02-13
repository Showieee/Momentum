using Momentum.Domain.Enums;

namespace Momentum.API.Models
{
    public class InsertProductRequest
    {
        public ProductType Type { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required decimal Price { get; set; }
        public required Guid CompanyId { get; set; }
    }
}
