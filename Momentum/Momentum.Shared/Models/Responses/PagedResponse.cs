namespace Momentum.Shared.Models.Responses;

public sealed class PagedResponse<T>
{
	#region Constructors

	public PagedResponse()
	{
		Data = new List<T>();
		PageNumber = 1;
		PageSize = 10;
	}

	public PagedResponse(IList<T>? data, int page = 1, int pageSize = 10, string[]? orderBy = null, int totalCount = 0)
	{
		Data = data ?? new List<T>();
		PageNumber = page;
		PageSize = pageSize;
		OrderBy = orderBy;
		TotalCount = totalCount;
	}

	#endregion //Constructors

	#region Public Properties

	public int PageNumber { get; set; }

	public int PageSize { get; set; }

	public string[]? OrderBy { get; set; }

	public int TotalCount { get; set; }

	public int StartIndex => PageNumber > 0 ? (PageNumber - 1) * PageSize : 0;

	public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;
	
	public IList<T> Data { get; set; }

	#endregion //Public Properties
}