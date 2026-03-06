using PersonsCompany = Momentum.BackOffice.Features.Companies;
using PersonsFeature = Momentum.BackOffice.Features.Persons;
using Momentum.Shared.Models.Requests;

namespace Momentum.BackOffice.Extensions;

public static class MappingExtensions
{
    public static InsertAddressRequest ToRequest(this PersonsFeature.AddressViewModel value)
    {
        return new InsertAddressRequest
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

    public static InsertAddressRequest ToRequest(this PersonsFeature.AddressEntryViewModel value)
    {
        return new InsertAddressRequest
        {
            StreetName = value.StreetName ?? string.Empty,
            StreetNumber = value.StreetNumber ?? string.Empty,
            City = value.City ?? string.Empty,
            State = value.State ?? string.Empty,
            Country = value.Country ?? string.Empty,
            Email = value.Email ?? string.Empty,
            PhoneNumber = value.PhoneNumber
        };
    }

    public static InsertPersonRequest ToRequest(this PersonsFeature.PersonEntryViewModel value)
    {
        return new InsertPersonRequest
        {
            FirstName = value.FirstName!,
            LastName = value.LastName!,
            CNP = value.CNP!,
            Address = value.Address?.ToRequest()
        };
    }

    public static InsertCompanyRequest ToRequest(this PersonsCompany.CompanyEntryViewModel value)
    {
        return new InsertCompanyRequest
        {
            Name = value.Name!,
            CUI = value.CUI!,
            Address = value.Address != null && !IsEmptyCompanyAddress(value.Address) ? new()
            {
                StreetName = value.Address.StreetName ?? string.Empty,
                StreetNumber = value.Address.StreetNumber ?? string.Empty,
                City = value.Address.City ?? string.Empty,
                State = value.Address.State ?? string.Empty,
                Country = value.Address.Country ?? string.Empty,
                Email = value.Address.Email ?? string.Empty,
                PhoneNumber = value.Address.PhoneNumber
            } : null
        };
    }

    private static bool IsEmptyCompanyAddress(PersonsCompany.AddressEntryViewModel address)
    {
        return string.IsNullOrEmpty(address.StreetName) &&
               string.IsNullOrEmpty(address.StreetNumber) &&
               string.IsNullOrEmpty(address.City) &&
               string.IsNullOrEmpty(address.State) &&
               string.IsNullOrEmpty(address.Country) &&
               string.IsNullOrEmpty(address.Email);
    }
}


