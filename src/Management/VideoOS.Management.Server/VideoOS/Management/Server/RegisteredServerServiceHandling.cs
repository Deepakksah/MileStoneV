using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using VideoOS.Common.Integration;
using VideoOS.Common.Services;
using VideoOS.Management.Common;
using VideoOS.Utilities.Network;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public static class RegisteredServerServiceHandling
{
	public static bool RegisterService(IGeminiRequestContext requestContext, Guid type, Guid serverId, string[] uri, string name, string description, string customData)
	{
		RegisteredServer.RegisterService(requestContext, type, serverId, GetAddresses(uri), name, description, customData);
		return true;
	}

	public static bool RegisterServiceWithEndpoints(IGeminiRequestContext requestContext, Guid type, Guid serverId, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		RegisteredServer.RegisterServiceWithEndpoints(requestContext, type, serverId, GetAddresses(uri), name, description, customData, endpointDefinition);
		return true;
	}

	public static bool SetServiceData(IGeminiRequestContext requestContext, Guid instance, string customData)
	{
		RegisteredServer.SetServiceData(requestContext, instance, customData);
		return true;
	}

	public static ServiceInfoHandler[] GetServices(IGeminiRequestContext requestContext, string locationIPAddress, bool includeDisabled)
	{
		RegisteredServer[] array = RegisteredServer.QueryAllRegisteredServers(requestContext, includeDisabled);
		List<ServiceInfoHandler> list = new List<ServiceInfoHandler>();
		bool isExternalRequest = IPHelper.IsNatRequest(locationIPAddress, requestContext.LocalIPAddress);
		isExternalRequest = GetIsExternalRequest(requestContext, isExternalRequest, locationIPAddress);
		RegisteredServer[] array2 = array;
		foreach (RegisteredServer registeredServer in array2)
		{
			EndpointDefinitionCollection fromXml = EndpointDefinitionCollection.GetFromXml(registeredServer.EndpointDefition);
			CreateServiceInfoObject(isExternalRequest, registeredServer, list, fromXml);
		}
		CreateServiceInfoObject(requestContext, list, isExternalRequest);
		return list.ToArray();
	}

	private static ServerAddress[] GetAddresses(string[] uri)
	{
		ServerAddresses serverAddresses = new ServerAddresses();
		foreach (string text in uri)
		{
			if (!Uri.TryCreate(text, UriKind.Absolute, out var _))
			{
				throw new UriFormatException("URI is not well formed or not absolute");
			}
			serverAddresses.Add(new ServerAddress(text, ServerAddressType.Internal));
		}
		return serverAddresses.ToArray();
	}

	private static ServiceEndpointHandler CreateServiceEndpointObject(AuthenticationType authenticationMode, string name, string description, string uri)
	{
		return new ServiceEndpointHandler
		{
			authentication = authenticationMode.ToString(),
			name = name,
			description = description,
			uri = uri
		};
	}

	private static void CreateServiceInfoObject(IGeminiRequestContext requestContext, List<ServiceInfoHandler> serviceInfos, bool isExternalRequest)
	{
		ServiceInfoHandler serviceInfoHandler = new ServiceInfoHandler();
		ServerInformation serverInformation = ServerInformation.GetServerInformation(requestContext);
		EndpointDefinitionCollection fromXml;
		using (IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent())
		{
			fromXml = EndpointDefinitionCollection.GetFromXml(managementServerComponent.GetSystemSetting(SettingList.ServerEndpointDefinitions));
		}
		serviceInfoHandler.instance = new Guid(serverInformation.Id);
		serviceInfoHandler.type = ServiceTypes.ServiceType_CorporateService;
		serviceInfoHandler.name = "Management Server";
		serviceInfoHandler.description = serverInformation.Description;
		serviceInfoHandler.enabled = true;
		serviceInfoHandler.endpointdefinition = EndpointDefinitionCollection.GetAsXml(fromXml);
		List<Uri> list = new List<Uri>();
		ServerAddress[] addresses = serverInformation.Addresses;
		foreach (ServerAddress serverAddress in addresses)
		{
			if (isExternalRequest && serverAddress.Type == ServerAddressType.External)
			{
				list.Add(new Uri(serverAddress.URL));
			}
			else if ((!isExternalRequest && serverAddress.Type == ServerAddressType.Internal) || (!isExternalRequest && serverAddress.Type == ServerAddressType.Server))
			{
				list.Add(new Uri(serverAddress.URL));
			}
		}
		if (list.Count == 0)
		{
			addresses = serverInformation.Addresses;
			foreach (ServerAddress serverAddress2 in addresses)
			{
				list.Add(new Uri(serverAddress2.URL));
			}
		}
		serviceInfoHandler.uri = list.Select((Uri f) => f.ToString()).ToArray();
		serviceInfoHandler.trusted = true;
		List<ServiceEndpointHandler> list2 = new List<ServiceEndpointHandler>();
		foreach (Uri item2 in list)
		{
			foreach (EndpointDefinition item3 in fromXml)
			{
				ServiceEndpointHandler item = CreateServiceEndpointObject(item3.AuthenticationType, item3.Name, item3.Description, item3.GetEndpointUri(item2));
				list2.Add(item);
			}
		}
		serviceInfoHandler.endpoints = list2.ToArray();
		serviceInfos.Add(serviceInfoHandler);
	}

	private static void CreateServiceInfoObject(bool isExternalRequest, RegisteredServer server, List<ServiceInfoHandler> serviceInfos, EndpointDefinitionCollection endpoints)
	{
		ServiceInfoHandler serviceInfoHandler = new ServiceInfoHandler();
		serviceInfoHandler.instance = server.IDServer;
		serviceInfoHandler.type = server.Type;
		serviceInfoHandler.name = server.Name;
		serviceInfoHandler.description = server.Description;
		serviceInfoHandler.enabled = server.Enabled;
		serviceInfoHandler.endpointdefinition = server.EndpointDefition;
		serviceInfoHandler.customData = server.CustomData;
		serviceInfoHandler.dataCollectorId = server.DataCollectorId;
		List<string> list = new List<string>();
		ServerAddress[] addresses = server.Addresses;
		foreach (ServerAddress serverAddress in addresses)
		{
			if (isExternalRequest && serverAddress.Type == ServerAddressType.External)
			{
				list.Add(serverAddress.URL);
			}
			else if (!isExternalRequest && serverAddress.Type == ServerAddressType.Internal)
			{
				list.Add(serverAddress.URL);
			}
		}
		if (list.Count == 0)
		{
			addresses = server.Addresses;
			foreach (ServerAddress serverAddress2 in addresses)
			{
				list.Add(serverAddress2.URL);
			}
		}
		serviceInfoHandler.uri = list.ToArray();
		List<ServiceEndpointHandler> list2 = new List<ServiceEndpointHandler>();
		foreach (string item2 in list)
		{
			foreach (EndpointDefinition endpoint in endpoints)
			{
				Uri uri = new Uri(item2);
				if (string.Equals(endpoint.Scheme, uri.Scheme, StringComparison.OrdinalIgnoreCase))
				{
					ServiceEndpointHandler item = CreateServiceEndpointObject(endpoint.AuthenticationType, endpoint.Name, endpoint.Description, endpoint.GetEndpointUri(uri));
					list2.Add(item);
				}
			}
		}
		serviceInfoHandler.endpoints = list2.ToArray();
		serviceInfoHandler.trusted = server.Trusted;
		serviceInfos.Add(serviceInfoHandler);
	}

	private static bool GetIsExternalRequest(IGeminiRequestContext requestContext, bool isExternalRequest, string locationIPAddress)
	{
		if (isExternalRequest)
		{
			string systemSetting;
			using (IManagementServerComponent managementServerComponent = requestContext.CreateManagementServerComponent())
			{
				systemSetting = managementServerComponent.GetSystemSetting(SettingList.LocalIPRanges);
			}
			if (IPAddressDefinedAsLocal(locationIPAddress, systemSetting))
			{
				isExternalRequest = false;
			}
		}
		return isExternalRequest;
	}

	private static bool IPAddressDefinedAsLocal(string address, string localIPRanges)
	{
		if (!string.IsNullOrEmpty(localIPRanges) && IPAddress.TryParse(address, out var address2))
		{
			foreach (LocalIPRangeXmlFormatter.IPRange value in LocalIPRangeXmlFormatter.FromXml(localIPRanges).IPRanges.Values)
			{
				if (value.RangeFrom.AddressFamily == address2.AddressFamily && IPHelper.IPAddressCompare(value.RangeFrom, address2) != 1 && IPHelper.IPAddressCompare(address2, value.RangeTo) != 1)
				{
					return true;
				}
			}
		}
		return false;
	}
}
