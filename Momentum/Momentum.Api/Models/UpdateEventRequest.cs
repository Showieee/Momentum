using Momentum.Domain.Enums;

namespace Momentum.API.Models
{
    public class UpdateEventRequest
    {
        public string? Name { get; set; }
        public string? Date { get; set; }
        public EventType? EventType { get; set; }


    }
}
