using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Entities;

namespace VideoOS.IdentityServer.Data;

public interface IApplicationDbContext
{
	DbSet<AdminIdentity> AdminIdentities { get; set; }

	DbSet<EventLog> EventLogs { get; set; }

	DbSet<LoginProvider> LoginProviders { get; set; }

	DbSet<LoginProviderClient> LoginProviderClients { get; set; }

	DbSet<TrustedIssuer> TrustedIssuers { get; set; }
}
