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

    public static CompanyEntity ToEntity(this CompanyModel value)
    {
        return new CompanyEntity
        {
            Address = value.Address?.ToEntity(),
            CUI = value.CUI,
            Name = value.Name
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

    public static CompanyModel ToModel(this CompanyEntity value)
    {
        return new CompanyModel
        {
            Id = value.Id,
            AddressId = value.AddressId,
            Address = value.Address?.ToModel(),
            CUI = value.CUI,
            Name = value.Name
        };
    }

    public static void MapToEntity(this AddressModel value, AddressEntity entity)
    {
        entity.StreetNumber = value.StreetNumber;
        entity.City = value.City;
        entity.State = value.State;
        entity.Country = value.Country;
        entity.Email = value.Email;
        entity.PhoneNumber = value.PhoneNumber;
        entity.StreetName = value.StreetName;
    }
}
