using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;

public class AddressConfiguration : IEntityTypeConfiguration<AddressEntity>
{
	public void Configure(EntityTypeBuilder<AddressEntity> builder)
	{
	}
}
