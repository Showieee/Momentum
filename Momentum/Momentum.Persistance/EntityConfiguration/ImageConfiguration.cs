using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;
public class ImageConfiguration : IEntityTypeConfiguration<ImageEntity>
{
	public void Configure(EntityTypeBuilder<ImageEntity> builder)
	{
	}
}
