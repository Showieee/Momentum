using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Momentum.Domain.Entities;

namespace Momentum.Persistance.EntityConfiguration;

public class PersonConfiguration : IEntityTypeConfiguration<PersonEntity>
{
	public void Configure(EntityTypeBuilder<PersonEntity> builder)
	{
	}
}
