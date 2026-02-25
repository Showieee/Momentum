using Momentum.Domain.Enums;

namespace Momentum.Services.Models
{
    public class EventModel
    {
        public Guid Id { get; set; }
        public EventType Type { get; set; }
        public required string Name { get; set; }
        public required string Date { get; set; }
        public List<ProductModel>? Products { get; set; } = [];
    }
}
