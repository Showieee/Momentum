namespace Momentum.Shared.Models.Requests;

public class InsertCompanyRequest
{
    public InsertAddressRequest? Address { get; set; }
    public required string Name { get; set; }
    public required string CUI { get; set; }
}

public class UpdateCompanyRequest
{
    public required Guid Id { get; set; }
    public UpdateAddressRequest? Address { get; set; }
    public required string Name { get; set; }
    public required string CUI { get; set; }
}
