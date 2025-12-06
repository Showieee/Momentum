using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;

public class CompanyConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
	public void Configure(EntityTypeBuilder<CompanyEntity> builder)
	{
		builder
			.HasMany(e => e.Products)
			.WithOne(e => e.Company)
			.HasForeignKey(e => e.CompanyId)
			.HasPrincipalKey(e => e.Id);
	}
}
