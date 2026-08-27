using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.Common.Integration;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServiceRegistrationService : GeminiWebServiceBase, IServiceRegistrationService
{
	public ServiceRegistrationService()
		: base(connect: false)
	{
	}

	public bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterService");
			methodInformation.AddParameter("type", type);
			methodInformation.AddParameter("instance", instance);
			methodInformation.AddArrayParameter("uri", uri);
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("customData", customData);
			EnterWebMethod(methodInformation);
			return RegisteredServerServiceHandling.RegisterService(base.RequestContext, type, instance, uri, name, description, customData);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterServiceWithEndpoints");
			methodInformation.AddParameter("type", type);
			methodInformation.AddParameter("instance", instance);
			methodInformation.AddArrayParameter("uri", uri);
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("customData", customData);
			methodInformation.AddParameter("endpointDefinition", endpointDefinition);
			EnterWebMethod(methodInformation);
			return RegisteredServerServiceHandling.RegisterServiceWithEndpoints(base.RequestContext, type, instance, uri, name, description, customData, endpointDefinition);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UnregisterService(Guid instance)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UnregisterService");
			methodInformation.AddParameter("instance", instance);
			EnterWebMethod(methodInformation);
			RegisteredServer.UnregisterService(base.RequestContext, instance);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void EnableService(Guid instance, bool enabled)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EnableService");
			methodInformation.AddParameter("instance", instance);
			methodInformation.AddParameter("enabled", enabled);
			EnterWebMethod(methodInformation);
			RegisteredServer.EnableService(base.RequestContext, instance, enabled);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServiceInfo[] GetServices()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServices");
			EnterWebMethod(methodInformation);
			BasicCallContextInfo callContextInfo = GetCallContextInfo();
			return ConvertToServiceInfo(RegisteredServerServiceHandling.GetServices(base.RequestContext, callContextInfo.LocationIPAddress, includeDisabled: false));
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ServiceInfo[] GetServicesIncludingDisabled()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServicesIncludingDisabled");
			EnterWebMethod(methodInformation);
			BasicCallContextInfo callContextInfo = GetCallContextInfo();
			return ConvertToServiceInfo(RegisteredServerServiceHandling.GetServices(base.RequestContext, callContextInfo.LocationIPAddress, includeDisabled: true));
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool SetServiceData(Guid instance, string customData)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetServiceData");
			methodInformation.AddParameter("instance", instance);
			methodInformation.AddParameter("customData", customData);
			EnterWebMethod(methodInformation);
			return RegisteredServerServiceHandling.SetServiceData(base.RequestContext, instance, customData);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetServiceData(Guid instance)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServiceData");
			methodInformation.AddParameter("instance", instance);
			EnterWebMethod(methodInformation);
			return RegisteredServer.GetServiceData(base.RequestContext, instance);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private BasicCallContextInfo GetCallContextInfo()
	{
		return new BasicCallContextInfo(base.RequestContext.AuthenticatedUserSid, base.RequestContext.UserDomainAccountName, base.RequestContext.RemoteIPAddress);
	}

	private static ServiceInfo[] ConvertToServiceInfo(ServiceInfoHandler[] services)
	{
		List<ServiceInfo> list = new List<ServiceInfo>();
		foreach (ServiceInfoHandler serviceInfoHandler in services)
		{
			list.Add(new ServiceInfo
			{
				name = serviceInfoHandler.name,
				type = serviceInfoHandler.type,
				description = serviceInfoHandler.description,
				instance = serviceInfoHandler.instance,
				uri = serviceInfoHandler.uri,
				enabled = serviceInfoHandler.enabled,
				endpointdefinition = serviceInfoHandler.endpointdefinition,
				endpoints = ConvertToServiceEndpoints(serviceInfoHandler.endpoints),
				trusted = serviceInfoHandler.trusted
			});
		}
		return list.ToArray();
	}

	private static ServiceEndpoint[] ConvertToServiceEndpoints(ServiceEndpointHandler[] endpoints)
	{
		List<ServiceEndpoint> list = new List<ServiceEndpoint>();
		foreach (ServiceEndpointHandler serviceEndpointHandler in endpoints)
		{
			list.Add(new ServiceEndpoint
			{
				name = serviceEndpointHandler.name,
				authentication = serviceEndpointHandler.authentication,
				description = serviceEndpointHandler.description,
				uri = serviceEndpointHandler.uri
			});
		}
		return list.ToArray();
	}
}
