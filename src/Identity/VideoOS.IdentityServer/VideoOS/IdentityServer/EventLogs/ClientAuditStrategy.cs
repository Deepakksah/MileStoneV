using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.EventLogInfrastructure;

namespace VideoOS.IdentityServer.EventLogs;

public sealed class ClientAuditStrategy(IEventLogFactory factory) : AuditStrategy<Client>
{
	private const string ClientId = "ClientId";

	private const string ClientName = "ClientName";

	protected override async Task<IEnumerable<EventLog>> ProcessEntityStateChange(EntityEntry entry, Client entity)
	{
		EventLog[] result;
		switch (entry.State)
		{
		case EntityState.Added:
		{
			EventLog eventLog2 = await factory.Create("RegistrationSuccess", entry.CurrentValues["ClientId"].ToString(), entry.CurrentValues["ClientName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
			result = new EventLog[1] { eventLog2 };
			break;
		}
		case EntityState.Deleted:
			result = Array.Empty<EventLog>();
			break;
		case EntityState.Modified:
		{
			EventLog eventLog = await factory.Create("RegistrationChanged", entry.CurrentValues["ClientId"].ToString(), entry.CurrentValues["ClientName"].ToString()).ConfigureAwait(continueOnCapturedContext: false);
			result = new EventLog[1] { eventLog };
			break;
		}
		default:
			result = Array.Empty<EventLog>();
			break;
		}
		return result;
	}
}
