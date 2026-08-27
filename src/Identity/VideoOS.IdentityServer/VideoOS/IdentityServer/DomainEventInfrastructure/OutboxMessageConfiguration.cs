using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class OutboxMessageConfiguration(bool includeInMigrations) : IEntityTypeConfiguration<OutboxMessage>
{
	public void Configure(EntityTypeBuilder<OutboxMessage> builder)
	{
		if (includeInMigrations)
		{
			builder.ToTable("OutboxMessages");
		}
		else
		{
			builder.ToTable("OutboxMessages", delegate(TableBuilder<OutboxMessage> t)
			{
				t.ExcludeFromMigrations();
			});
		}
		builder.HasKey((OutboxMessage e) => e.Id);
		builder.Property((OutboxMessage e) => e.Id).IsRequired().HasDefaultValueSql("NEWSEQUENTIALID()");
		builder.Property((OutboxMessage e) => e.EventType).IsRequired().HasMaxLength(200);
		builder.Property((OutboxMessage e) => e.Payload).IsRequired();
		builder.Property((OutboxMessage e) => e.CompatibilityLevel).IsRequired();
		builder.Property((OutboxMessage e) => e.Status).IsRequired();
		builder.Property((OutboxMessage e) => e.CreatedAtUtc).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
		builder.Property((OutboxMessage e) => e.ProcessedAtUtc).IsRequired(required: false);
		builder.Property((OutboxMessage e) => e.ProcessingInstanceId).IsRequired(required: false).HasMaxLength(36);
		builder.Property((OutboxMessage e) => e.LockExpiresAtUtc).IsRequired(required: false);
		builder.Property((OutboxMessage e) => e.LastAttemptUtc).IsRequired(required: false);
		builder.Property((OutboxMessage e) => e.RetryCount).IsRequired().HasDefaultValue(0);
		builder.Property((OutboxMessage e) => e.Error).IsRequired(required: false);
		builder.HasIndex((OutboxMessage e) => new { e.Status, e.CompatibilityLevel, e.Id }).HasDatabaseName("IX_OutboxMessages_New").IncludeProperties((OutboxMessage e) => new { e.ProcessingInstanceId, e.LockExpiresAtUtc, e.CreatedAtUtc });
		builder.HasIndex((OutboxMessage e) => new { e.Status, e.CompatibilityLevel, e.LastAttemptUtc, e.Id }).HasDatabaseName("IX_OutboxMessages_Retry").IncludeProperties((OutboxMessage e) => new { e.ProcessingInstanceId, e.LockExpiresAtUtc, e.RetryCount, e.CreatedAtUtc });
	}
}
