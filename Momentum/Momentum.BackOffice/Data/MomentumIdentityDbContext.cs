using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Momentum.BackOffice.Data;
public class MomentumIdentityDbContext(DbContextOptions<MomentumIdentityDbContext> options)
	: IdentityDbContext<ApplicationUser>(options);