namespace Momentum.Shared.Models.Requests;

public class PagingAndOrderingRequest: IPagingAndOrderingRequest
{
	public int PageNumber { get; set; } = 1;

	public int PageSize { get; set; } = 20;

	public string[]? OrderBy { get; set; }

	public int StartIndex => PageNumber > 0 ? (PageNumber - 1) * PageSize : 0;
}