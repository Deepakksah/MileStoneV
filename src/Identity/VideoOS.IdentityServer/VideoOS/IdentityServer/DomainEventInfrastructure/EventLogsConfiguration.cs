using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.DomainEventInfrastructure;

public class EventLogsConfiguration(bool includeInMigrations) : IEntityTypeConfiguration<EventLog>
{
	public void Configure(EntityTypeBuilder<EventLog> builder)
	{
		if (includeInMigrations)
		{
			builder.ToTable("EventLogs");
			return;
		}
		builder.ToTable("EventLogs", delegate(TableBuilder<EventLog> t)
		{
			t.ExcludeFromMigrations();
		});
	}
}
