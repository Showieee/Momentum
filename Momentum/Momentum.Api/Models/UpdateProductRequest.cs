using Momentum.Domain.Enums;

namespace Momentum.API.Models
{
    public class UpdateProductRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public ProductType? ProductType { get; set; }
    }
}
