using Momentum.BackOffice.Features.Persons;
using Momentum.Shared.Models.Requests;

namespace Momentum.BackOffice.Extensions;

public static class MappingExtensions
{
    public static InsertAddressRequest ToRequest(this AddressViewModel value)
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

    public static InsertPersonRequest ToRequest(this PersonEntryViewModel value)
    {

        return new InsertPersonRequest
        {
            FirstName = value.FirstName!,
            LastName = value.LastName!,
            CNP = value.CNP!,
            //Address = value.Address?.ToRequest()
        };
    }
}
