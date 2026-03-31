using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;

namespace Momentum.Persistance;

public class MomentumDbContext : DbContext
{

	public MomentumDbContext()
	{
		
	}

	public MomentumDbContext(DbContextOptions options) : base(options)
	{

	}

	public virtual DbSet<ProductEntity> Products { get; set; } = null!;
	public virtual DbSet<CompanyEntity> Companies { get; set; } = null!;
	public virtual DbSet<AddressEntity> Addresses { get; set; } = null!;
	public virtual DbSet<ImageEntity> Images { get; set; } = null!;
	public virtual DbSet<PersonEntity> Persons { get; set; } = null!;
	public virtual DbSet<EventEntity> Events { get; set; } = null!;
	public virtual DbSet<EventOrderEntity> EventOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
	}
}
