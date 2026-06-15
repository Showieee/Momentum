using Momentum.Domain.Entities;
using Momentum.Domain.Enums;

namespace Momentum.Services.Models
{
    public class ProductModel
    {
        public Guid Id { get; set; }
        public ProductType Type { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public bool IsPerPerson { get; set; }
        public bool IsHourly { get; set; }
        public Guid CompanyId { get; set; }
        public CompanyModel? Company { get; set; } = null!;
    }
}
