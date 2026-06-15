using Momentum.API.Models;
using Momentum.Domain.Enums;
using Momentum.Services.Models;

namespace Momentum.API.Extensions;

public static class MappingExtensions
{
    public static EventModel ToModel(this InsertEventRequest value)
    {
        return new EventModel
        {   
            Type = value.Type,
            Name = value.Name,
            Date = value.Date
        };
    }
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
            IsPerPerson = value.IsPerPerson,
            IsHourly = value.IsHourly,
			CompanyId = value.CompanyId
        };
    }

    public static PersonModel ToModel(this InsertPersonRequest value)
    {

        return new PersonModel
        {
            FirstName = value.FirstName,
            LastName = value.LastName,
            CNP = value.CNP,
            Address = value.Address?.ToModel()
        };
    }

    public static PersonModel ToModel(this UpdatePersonRequest value)
    {
        return new PersonModel
        {
            FirstName = value.FirstName ?? string.Empty,
            LastName = value.LastName ?? string.Empty,
            CNP = value.CNP ?? string.Empty,
            Address = value.Address
        };
    }

    public static Models.CreateEventOrderRequest ToModel(this Models.CreateEventOrderRequest value)
    {
        return new Models.CreateEventOrderRequest
        {
            PersonId = value.PersonId,
            EventId = value.EventId,
            Products = value.Products.Select(p => new Models.EventOrderProductRequest
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
                NumberOfPeople = p.NumberOfPeople,
                NumberOfHours = p.NumberOfHours
            }).ToList()
        };
    }

    public static ProductModel ToModel(this UpdateProductRequest value)
    {
        return new ProductModel
        {
            Type = value.Type ?? ProductType.Undefined,
            Name = value.Name ?? string.Empty,
            Description = value.Description,
            Price = value.Price ?? 0,
            IsPerPerson = value.IsPerPerson ?? false,
            IsHourly = value.IsHourly ?? false
        };
    }

    public static EventModel ToModel(this UpdateEventRequest value)
    {
        return new EventModel
        {
            Type = value.EventType ?? EventType.Undefined,
            Name = value.Name ?? string.Empty,
            Date = value.Date ?? string.Empty,
        };
    }
}
