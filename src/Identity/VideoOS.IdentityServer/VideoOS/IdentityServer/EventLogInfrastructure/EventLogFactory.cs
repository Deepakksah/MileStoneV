using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using VideoOS.IdentityServer.Entities;
using VideoOS.IdentityServer.Repositories.Interfaces;

namespace VideoOS.IdentityServer.EventLogInfrastructure;

public class EventLogFactory(IHttpContextAccessor httpContextAccessor, IConfigurationRepository clientRepository) : IEventLogFactory
{
	private readonly Dictionary<string, string> _clients = new Dictionary<string, string>();

	public async Task<EventLog> Create(string eventName, string objectId, string objectName)
	{
		string clientId = httpContextAccessor.HttpContext.GetClientId();
		EventLog eventLog = new EventLog
		{
			TimeStamp = DateTimeOffset.UtcNow,
			Event = eventName,
			UserName = httpContextAccessor.HttpContext.GetAuditUserIdentification()
		};
		HttpContext httpContext = httpContextAccessor.HttpContext;
		string remoteIpAddress;
		if (httpContext != null)
		{
			ConnectionInfo connection = httpContext.Connection;
			if (connection != null && connection.RemoteIpAddress != null)
			{
				remoteIpAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
				goto IL_00ba;
			}
		}
		remoteIpAddress = string.Empty;
		goto IL_00ba;
		IL_00ba:
		eventLog.RemoteIpAddress = remoteIpAddress;
		eventLog.ObjectId = objectId;
		eventLog.ObjectName = objectName;
		eventLog.ClientId = clientId;
		EventLog eventLog2 = eventLog;
		eventLog2.ClientName = await GetClientName(clientId).ConfigureAwait(continueOnCapturedContext: false);
		return eventLog;
	}

	private async Task<string> GetClientName(string clientId)
	{
		string value = null;
		if (clientId != null && !_clients.TryGetValue(clientId, out value))
		{
			value = (await clientRepository.GetByClientIdAsync(clientId).ConfigureAwait(continueOnCapturedContext: false))?.ClientName;
			_clients[clientId] = value;
		}
		return value;
	}
}
