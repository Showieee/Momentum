namespace Momentum.BackOffice.Services.Models;

public sealed record GetPaymentsResponse
{
	public decimal Card { get; set; }
	public int NrOfCardPayments { get; set; }
	public decimal Cash { get; set; }
	public int NrOfCashPayments { get; set; }
	public decimal Total { get; set; }
}
