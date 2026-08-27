using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.ServiceRegistrationService;

public class ServiceRegistrationServiceProxy : WebServiceManagerBase<ServiceRegistrationServiceClient, IServiceRegistrationService>, IServiceRegistrationServiceProxy, ITimeoutSettings, IAdditionalMessageHeaders, IDisposable
{
	public static IServiceRegistrationServiceProxy CreateNew(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServiceRegistrationServiceProxy(hostName, port, runSecure, tokenCache, exceptionHandler);
	}

	private ServiceRegistrationServiceProxy(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(runSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServiceRegistrationServiceOAuth.svc").Uri, "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", tokenCache, exceptionHandler)
	{
	}

	public static IServiceRegistrationServiceProxy CreateNew(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServiceRegistrationServiceProxy(hostName, port, exceptionHandler);
	}

	public static IServiceRegistrationServiceProxy CreateNew(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServiceRegistrationServiceProxy(hostName, port, userName, password, domain, exceptionHandler);
	}

	public static IServiceRegistrationServiceProxy CreateNew(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServiceRegistrationServiceProxy(hostName, securePort, userName, password, exceptionHandler);
	}

	private ServiceRegistrationServiceProxy(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServiceRegistrationService.svc").Uri, "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", exceptionHandler)
	{
	}

	private ServiceRegistrationServiceProxy(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServiceRegistrationService.svc").Uri, "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", userName, password, domain, exceptionHandler)
	{
	}

	private ServiceRegistrationServiceProxy(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttps, hostName, securePort, "ManagementServer/ServiceRegistrationService.svc").Uri, "http://schemas.videoos/ServiceRegistrationService/2008/06/Vmo/01", userName, password, exceptionHandler)
	{
	}

	protected override ServiceRegistrationServiceClient CreateClientObject(Binding binding, EndpointAddress remoteAddress)
	{
		return new ServiceRegistrationServiceClient(binding, remoteAddress);
	}

	void IAdditionalMessageHeaders.AddMessageHeader(string ns, string name, string content)
	{
		AddMessageHeader(ns, name, content);
	}

	bool IServiceRegistrationServiceProxy.RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData)
	{
		return CallWithRetry((IServiceRegistrationService c) => c.RegisterService(type, instance, uri, name, description, customData));
	}

	bool IServiceRegistrationServiceProxy.RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		return CallWithRetry((IServiceRegistrationService c) => c.RegisterServiceWithEndpoints(type, instance, uri, name, description, customData, endpointDefinition));
	}

	void IServiceRegistrationServiceProxy.UnregisterService(Guid instance)
	{
		CallWithRetry(delegate(IServiceRegistrationService c)
		{
			c.UnregisterService(instance);
		});
	}

	void IServiceRegistrationServiceProxy.EnableService(Guid instance, bool enabled)
	{
		CallWithRetry(delegate(IServiceRegistrationService c)
		{
			c.EnableService(instance, enabled);
		});
	}

	ServiceInfo[] IServiceRegistrationServiceProxy.GetServices()
	{
		return CallWithRetry((IServiceRegistrationService c) => c.GetServices());
	}

	ServiceInfo[] IServiceRegistrationServiceProxy.GetServicesIncludingDisabled()
	{
		return CallWithRetry((IServiceRegistrationService c) => c.GetServicesIncludingDisabled());
	}

	bool IServiceRegistrationServiceProxy.SetServiceData(Guid instance, string customData)
	{
		return CallWithRetry((IServiceRegistrationService c) => c.SetServiceData(instance, customData));
	}

	string IServiceRegistrationServiceProxy.GetServiceData(Guid instance)
	{
		return CallWithRetry((IServiceRegistrationService c) => c.GetServiceData(instance));
	}
}
