using Momentum.Domain.Enums;
using Momentum.Services.Models;

namespace Momentum.API.Models
{
    public class InsertEventRequest
    {
        public EventType Type { get; set; }
        public required string Name { get; set; }
        public required string Date { get; set; }
    }
}
