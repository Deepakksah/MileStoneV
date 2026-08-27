using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Util.Svc;

public class ServiceRegistrationRestClientDecorator : IServiceRegistration, IDisposable
{
	private ServiceRegistrationClient serviceRegistrationClient;

	private const string allServices = "VMSinternal";

	public ServiceRegistrationRestClientDecorator()
	{
	}

	public static IServiceRegistration Create(LoginSettings loginSettings)
	{
		try
		{
			bool enableDebugLogging = ConfigurationApiManager.EnableDebugLogging;
			ServiceRegistrationClient serviceRegistrationClient = ServiceRegistrationClient.Create(loginSettings, enableDebugLogging);
			if (serviceRegistrationClient == null)
			{
				return null;
			}
			serviceRegistrationClient.SetOption("VMSinternal");
			return new ServiceRegistrationRestClientDecorator
			{
				serviceRegistrationClient = serviceRegistrationClient
			};
		}
		catch (MIPException)
		{
			return null;
		}
	}

	public void Dispose()
	{
		serviceRegistrationClient?.Dispose();
		serviceRegistrationClient = null;
	}

	public string GetServiceData(Guid instance)
	{
		return serviceRegistrationClient.GetService(instance)?.customData;
	}

	public VideoOS.Platform.Util.ServiceInfo[] GetServices()
	{
		List<VideoOS.Platform.Util.ServiceInfo> list = new List<VideoOS.Platform.Util.ServiceInfo>();
		foreach (ServiceInformation item in CallWithRetry((ServiceRegistrationClient client) => client.GetServices()))
		{
			list.Add(new VideoOS.Platform.Util.ServiceInfo
			{
				name = item.name,
				description = item.description,
				uri = new ArrayOfString(item.uris.ToArray()),
				type = item.type,
				trusted = item.trusted,
				instance = item.instance,
				endpoints = ConvertToRegistrationEndpoints(item.endpoints)
			});
		}
		return list.ToArray();
	}

	public bool RegisterService(Guid type, Guid instance, ArrayOfString uris, string name, string description, string customData)
	{
		CallWithRetryVoid(delegate(ServiceRegistrationClient client)
		{
			client.RegisterService(type, instance, ConvertToArrayOfUri(uris), name, description, customData);
		});
		return true;
	}

	public bool RegisterServiceWithEndpoints(Guid type, Guid instance, ArrayOfString uris, string name, string description, string customData, string endpointDefinitions)
	{
		CallWithRetryVoid(delegate(ServiceRegistrationClient client)
		{
			client.RegisterService(type, instance, ConvertToArrayOfUri(uris), name, description, customData, ConvertEndpointDefinition(endpointDefinitions));
		});
		return true;
	}

	public bool SetServiceData(Guid instance, string customData)
	{
		CallWithRetryVoid(delegate(ServiceRegistrationClient client)
		{
			client.UpdateService(instance, null, customData, null);
		});
		return true;
	}

	public void UnregisterService(Guid instance)
	{
		CallWithRetryVoid(delegate(ServiceRegistrationClient client)
		{
			client.UnregisterService(instance);
		});
	}

	private static ArrayOfEndpointDefinition ConvertToRegistrationEndpoints(List<ServiceInformationEndpoint> endpoints)
	{
		ArrayOfEndpointDefinition arrayOfEndpointDefinition = new ArrayOfEndpointDefinition();
		foreach (ServiceInformationEndpoint endpoint in endpoints)
		{
			arrayOfEndpointDefinition.Add(new EndpointDefinition(endpoint.uri, endpoint.authentication, endpoint.name, endpoint.description));
		}
		return arrayOfEndpointDefinition;
	}

	private static Uri[] ConvertToArrayOfUri(ArrayOfString array)
	{
		List<Uri> list = new List<Uri>();
		foreach (string item in array)
		{
			list.Add(new Uri(item));
		}
		return list.ToArray();
	}

	private static List<ServiceInformationEndpoint> ConvertEndpointDefinition(string definition)
	{
		EndpointDefinitions fromXml = EndpointDefinitions.GetFromXml(definition);
		List<ServiceInformationEndpoint> list = new List<ServiceInformationEndpoint>();
		foreach (EndpointDefinition item in fromXml)
		{
			list.Add(new ServiceInformationEndpoint
			{
				authentication = item.AuthenticationType,
				name = item.Name,
				description = item.Description,
				uri = item.ServerProvidedUri.ToString()
			});
		}
		return list;
	}

	private TResult CallWithRetry<TResult>(Func<ServiceRegistrationClient, TResult> call)
	{
		//IL_003c: Expected O, but got Unknown
		//IL_0013: Expected O, but got Unknown
		try
		{
			return call(serviceRegistrationClient);
		}
		catch (MIPException)
		{
			throw;
		}
		catch (HttpRequestException ex2)
		{
			HttpRequestException ex3 = ex2;
			if (IsUnauthorizedLogin(ex3))
			{
				throw new NotAuthorizedMIPException(((Exception)(object)ex3).Message);
			}
			try
			{
				return call(serviceRegistrationClient);
			}
			catch (HttpRequestException ex4)
			{
				throw new CommunicationMIPException(((Exception)ex4).Message);
			}
		}
		catch (ObjectDisposedException)
		{
			return call(serviceRegistrationClient);
		}
	}

	private void CallWithRetryVoid(Action<ServiceRegistrationClient> call)
	{
		//IL_003a: Expected O, but got Unknown
		//IL_0012: Expected O, but got Unknown
		try
		{
			call(serviceRegistrationClient);
		}
		catch (MIPException)
		{
			throw;
		}
		catch (HttpRequestException ex2)
		{
			HttpRequestException ex3 = ex2;
			if (IsUnauthorizedLogin(ex3))
			{
				throw new NotAuthorizedMIPException(((Exception)(object)ex3).Message);
			}
			try
			{
				call(serviceRegistrationClient);
			}
			catch (HttpRequestException ex4)
			{
				throw new CommunicationMIPException(((Exception)ex4).Message);
			}
		}
		catch (ObjectDisposedException)
		{
			call(serviceRegistrationClient);
		}
	}

	private static bool IsUnauthorizedLogin(HttpRequestException ex)
	{
		if (((Exception)(object)ex)?.InnerException?.InnerException is WebException ex2)
		{
			HttpWebResponse obj = ex2.Response as HttpWebResponse;
			if (obj == null)
			{
				return false;
			}
			return obj.StatusCode == HttpStatusCode.Unauthorized;
		}
		return false;
	}
}
