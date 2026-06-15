using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;

public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
{
	public void Configure(EntityTypeBuilder<ProductEntity> builder)
	{
		builder.Property(x => x.IsPerPerson)
			.HasDefaultValue(false);

		builder.Property(x => x.IsHourly)
			.HasDefaultValue(false);
	}
}
