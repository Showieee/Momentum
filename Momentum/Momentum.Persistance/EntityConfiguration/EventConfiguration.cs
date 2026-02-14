using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;

public class EventConfiguration : IEntityTypeConfiguration<EventEntity>
{
    public void Configure(EntityTypeBuilder<EventEntity> builder)
    {
        builder
            .HasMany(e => e.Products)
            .WithMany(e => e.Events)
            .UsingEntity(e => e.ToTable("EventProduct"));
    }
}
