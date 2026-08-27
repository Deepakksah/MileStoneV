using System;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Services;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class RegisteredServer
{
	public Guid IDServer { get; set; }

	public Guid Type { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public ServerAddress[] Addresses { get; set; }

	public bool Trusted { get; set; }

	public string CustomData { get; set; }

	public bool Enabled { get; set; }

	public int DataCollectorId { get; set; }

	public string EndpointDefition { get; set; }

	public static RegisteredServer QueryRegisteredServer(IGeminiRequestContext requestContext, Guid serverId)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		return registeredServerComponent.QueryRegisteredServer(serverId);
	}

	public static RegisteredServer[] QueryRegisteredServersByType(IGeminiRequestContext requestContext, Guid serviceType)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		return registeredServerComponent.QueryRegisteredServersByType(serviceType);
	}

	public static RegisteredServer[] QueryAllRegisteredServers(IGeminiRequestContext requestContext, bool includeDisabled)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		return registeredServerComponent.QueryAllRegisteredServers(includeDisabled);
	}

	public static RegisteredServer[] QueryRegisteredServersWithoutDataCollectorId(IGeminiRequestContext requestContext)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		string[] ruleServiceTypes = PerformanceRuleServiceTypes.RuleServiceTypes.Select((Guid x) => x.ToString()).ToArray();
		return registeredServerComponent.QueryRegisteredServersWithoutDataCollectorId(ruleServiceTypes);
	}

	public static void UpdateDataCollectorsForRegisteredServers(IGeminiRequestContext requestContext, Guid serverId, int dataCollectorId)
	{
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		registeredServerComponent.UpdateDataCollectors(serverId, dataCollectorId);
	}

	public static void EnableService(IGeminiRequestContext requestContext, Guid serverId, bool enabled)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		RegisteredServer registeredServer;
		using (IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent())
		{
			registeredServer = registeredServerComponent.QueryRegisteredServer(serverId);
			if (registeredServer == null)
			{
				return;
			}
			registeredServerComponent.EnableService(serverId, enabled);
		}
		Integration.TryNotifyServerRegistrationChangedAsync();
		if (registeredServer.Type == ServiceTypes.ServiceType_LogService)
		{
			Integration.TryNotifyServerLogServerChangedAsync();
		}
		DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
	}

	public static void RegisterService(IGeminiRequestContext requestContext, Guid type, Guid serverId, ServerAddress[] addresses, string name, string description, string customData)
	{
		RegisterServiceWithEndpoints(requestContext, type, serverId, addresses, name, description, customData, "<endpoints></endpoints>");
	}

	public static void RegisterServiceWithEndpoints(IGeminiRequestContext requestContext, Guid type, Guid serverId, ServerAddress[] addresses, string name, string description, string customData, string endpointDefinition)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using (IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent())
		{
			registeredServerComponent.RegisterService(type, serverId, addresses, name, description, customData, endpointDefinition);
		}
		Integration.TryNotifyServerRegistrationChangedAsync();
		if (type == ServiceTypes.ServiceType_LogService)
		{
			Integration.TryNotifyServerLogServerChangedAsync();
		}
		DataCollectorOnRegisteredServicesManager.Instance.QueueResolve();
	}

	public static void UnregisterService(IGeminiRequestContext requestContext, Guid serverId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		RegisteredServer registeredServer;
		using (IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent())
		{
			registeredServer = registeredServerComponent.QueryRegisteredServer(serverId);
			if (registeredServer == null)
			{
				return;
			}
			registeredServerComponent.UnregisterService(serverId);
		}
		Integration.TryNotifyServerRegistrationChangedAsync();
		if (registeredServer.Type == ServiceTypes.ServiceType_LogService)
		{
			Integration.TryNotifyServerLogServerChangedAsync();
		}
	}

	public static void SetServiceData(IGeminiRequestContext requestContext, Guid serverId, string customData)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent();
		registeredServerComponent.SetServiceData(serverId, customData);
	}

	public static string GetServiceData(IGeminiRequestContext requestContext, Guid serverId)
	{
		RegisteredServer registeredServer;
		using (IRegisteredServerComponent registeredServerComponent = requestContext.CreateRegisteredServerComponent())
		{
			registeredServer = registeredServerComponent.QueryRegisteredServer(serverId);
		}
		if (registeredServer != null && registeredServer.Enabled)
		{
			return registeredServer.CustomData;
		}
		return null;
	}
}
