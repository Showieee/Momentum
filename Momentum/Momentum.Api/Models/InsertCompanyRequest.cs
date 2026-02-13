namespace Momentum.API.Models;

public class InsertCompanyRequest
{
    public InsertAddressRequest? Address { get; set; } = null;
    public required string Name { get; set; }
    public required string CUI { get; set; }
}
