using Momentum.BackOffice.Services.Models;
using Refit;

namespace Momentum.BackOffice.Services;

public interface IOptionsService
{
	[Get(Momentum.Shared.Routes.Options.Get)]
	Task<IApiResponse<GetOptionsResponse>> GetOptions(CancellationToken ct = default);

	[Post(Momentum.Shared.Routes.Options.Upsert)]
	Task<IApiResponse<GetOptionsResponse>> UpsertOptions([Body] UpsertOptionsRequest upsertOptionsRequest, CancellationToken ct = default);
}