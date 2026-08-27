using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VideoOS.IdentityServer.DomainEventInfrastructure;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Extensions;
using VideoOS.IdentityServer.Repositories;

namespace VideoOS.IdentityServer.Data;

public class IdentityDbContextWrapper : IdentityDbContext<VmsUser, IdentityRole<string>, string, VmsUserClaim, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
	public IdentityDbContextWrapper(DbContextOptions<IdentityDbContextWrapper> options)
		: base((DbContextOptions)options)
	{
	}

	public IdentityDbContextWrapper()
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration(includeInMigrations: false));
		modelBuilder.ApplyConfiguration(new EventLogsConfiguration(includeInMigrations: false));
		modelBuilder.Entity<VmsUserClaim>().HasQueryFilter((VmsUserClaim p) => !BlacklistedUserClaims.Blacklist.Contains(p.ClaimType));
		modelBuilder.Entity(delegate(EntityTypeBuilder<VmsUser> b)
		{
			b.HasMany((VmsUser e) => e.Claims).WithOne().HasForeignKey((VmsUserClaim uc) => uc.UserId)
				.IsRequired();
			b.HasMany((VmsUser e) => e.Logins).WithOne().HasForeignKey((IdentityUserLogin<string> ul) => ul.UserId)
				.IsRequired();
		});
		ValueConverter<DateTime, DateTime> dateTimeConverter = new ValueConverter<DateTime, DateTime>((DateTime v) => v, (DateTime v) => DateTime.SpecifyKind(v, DateTimeKind.Utc));
		modelBuilder.EntitiesOfType<IModifiedAware>(delegate(EntityTypeBuilder builder)
		{
			builder.Property<DateTime>("CreatedDate").HasConversion(dateTimeConverter);
			builder.Property<DateTime>("ModifiedDate").HasConversion(dateTimeConverter);
		});
	}

	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IEnumerable<EntityEntry> enumerable = from e in ChangeTracker.Entries()
			where e.Entity is IModifiedAware && (e.State == EntityState.Modified || e.State == EntityState.Added)
			select e;
		foreach (EntityEntry item in enumerable)
		{
			DateTime utcNow = DateTime.UtcNow;
			((IModifiedAware)item.Entity).ModifiedDate = utcNow;
			if (item.State == EntityState.Added)
			{
				((IModifiedAware)item.Entity).CreatedDate = utcNow;
			}
		}
		return base.SaveChangesAsync(cancellationToken);
	}
}
