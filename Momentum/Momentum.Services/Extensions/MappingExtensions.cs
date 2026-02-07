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

    public static ProductEntity ToEntity(this ProductModel value, CompanyEntity companyEntity)
    {
        return new ProductEntity
        {
            Name = value.Name,
            Description = value.Description,
            Price = value.Price,
            Type = value.Type,
            Company = companyEntity
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

    public static CompanyModel ToModel(this CompanyEntity value, bool mapProducts = true)
    {
        return new CompanyModel
        {
            Id = value.Id,
            AddressId = value.AddressId,
            Address = value.Address?.ToModel(),
            CUI = value.CUI,
            Name = value.Name,
            Products = !mapProducts ? null : value.Products?.Select(p => p.ToModel(false)).ToList(),
        };
    }

    public static ProductModel ToModel(this ProductEntity value, bool mapCompanies = true)
    {
        return new ProductModel
        {
            Id = value.Id,
            Type = value.Type,
            Description = value.Description,
            CompanyId = value.CompanyId,
            Name = value.Name,
            Price = value.Price,
            Company = !mapCompanies ? null : value.Company.ToModel(false),
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

    public static void MapToEntity(this ProductModel value, ProductEntity entity)
    {
        entity.Name = value.Name;
        entity.Description = value.Description;
        entity.Price = value.Price;
        entity.Type = value.Type;
    }
}
