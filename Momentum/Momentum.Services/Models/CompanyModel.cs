namespace Momentum.Services.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; }
        public Guid? AddressId { get; set; }
        public AddressModel? Address { get; set; } = null;
        public required string Name { get; set; }
        public required string CUI { get; set; }
        public List<ProductModel>? Products { get; set; } = [];
    }
}
