using Momentum.Domain.Enums;
using Momentum.Services.Models;

namespace Momentum.API.Models
{
    public class UpdatePersonRequest
    {
        public AddressModel? Address { get; set; } = null;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CNP { get; set; }
        
    }
}
