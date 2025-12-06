using Momentum.API.Models;
using Momentum.Domain.Entities;
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
}
