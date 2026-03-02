namespace Momentum.Shared.Models.Requests;

public interface IPagingAndOrderingRequest
{
	int PageNumber { get; set; }

	int PageSize { get; set; }

	string[]? OrderBy { get; set; }
}