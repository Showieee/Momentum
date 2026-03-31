using Refit;

namespace Momentum.BackOffice.Services;

public interface IEventService
{
    [Get(Momentum.Shared.Routes.Events.Get)]
    Task<IApiResponse<List<GetEventResponse>>> GetEvents(CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.Events.Insert)]
    Task<IApiResponse> InsertEvent([Body] InsertEventRequest request, CancellationToken ct = default);

    [Put(Momentum.Shared.Routes.Events.Update)]
    Task<IApiResponse> UpdateEvent(Guid id, [Body] UpdateEventRequest request, CancellationToken ct = default);

    [Delete(Momentum.Shared.Routes.Events.Delete)]
    Task<IApiResponse> DeleteEvent(Guid id, CancellationToken ct = default);
}

public class InsertEventRequest
{
    public int Type { get; set; }
    public required string Name { get; set; }
    public required string Date { get; set; }
}

public class UpdateEventRequest
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public required string Name { get; set; }
    public required string Date { get; set; }
}

public class GetEventResponse
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}
