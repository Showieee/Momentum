using Momentum.Domain.Entities;
using Momentum.Services.Models;

namespace Momentum.Services.Extensions;
public static class MappingExtensions
{

	public static AddressEntity ToEntity(this AddressModel value)
	{
		return new AddressEntity
		{
			Id = value.Id,
			StreetName = value.StreetName,
			StreetNumber = value.StreetNumber,
			City = value.City,
			State = value.State,
			Country = value.Country,
			Email = value.Email,
			PhoneNumber = value.PhoneNumber
		};
	}

	public static AddressModel ToModel(this AddressEntity value)
	{
		return new AddressModel
		{
			Id = value.Id,
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
