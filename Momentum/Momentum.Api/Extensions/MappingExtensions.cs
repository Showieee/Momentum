using Momentum.API.Models;
using Momentum.Domain.Enums;
using Momentum.Services.Models;

namespace Momentum.API.Extensions;

public static class MappingExtensions
{
	public static AddressModel ToModel(this InsertAddressRequest value)
	{
		return new AddressModel
		{
			StreetName = value.StreetName,
			StreetNumber = value.StreetNumber,
			City = value.City,
			State = value.State,
			Country = value.Country,
			Email = value.Email,
			PhoneNumber = value.PhoneNumber
		};
	}
    public static CompanyModel ToModel(this InsertCompanyRequest value)
    {
		return new CompanyModel
		{
			Address = value.Address?.ToModel(),
			CUI = value.CUI,
			Name = value.Name
		};
    }

    public static ProductModel ToModel(this InsertProductRequest value)
    {
        
        return new ProductModel
        {
            Type = value.Type,
            Name = value.Name,
            Description = value.Description,
            Price = value.Price,
			CompanyId = value.CompanyId
        };
    }

    public static ProductModel ToModel(this UpdateProductRequest value)
    {
        return new ProductModel
        {
            Type = value.ProductType ?? ProductType.Undefined,
            Name = value.Name ?? string.Empty,
            Description = value.Description,
            Price = value.Price ?? 0
        };
    }
}
