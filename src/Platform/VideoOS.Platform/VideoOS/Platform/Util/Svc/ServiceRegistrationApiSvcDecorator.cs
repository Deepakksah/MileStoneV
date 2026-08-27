using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Threading;

namespace VideoOS.Platform.Util.Svc;

public class ServiceRegistrationApiSvcDecorator : IServiceRegistration, IDisposable
{
	private ServiceRegistrationServiceClient _serviceRegistrationService;

	public virtual bool IsBasicUser { get; set; }

	public virtual ICredentials Credentials { get; set; }

	public string Url { get; set; }

	public string GetServiceData(Guid instance)
	{
		return CallWithRetry((ServiceRegistrationServiceClient client) => client.GetServiceData(instance));
	}

	public VideoOS.Platform.Util.ServiceInfo[] GetServices()
	{
		ServiceInfo[] array = CallWithRetry((ServiceRegistrationServiceClient client) => client.GetServices());
		List<VideoOS.Platform.Util.ServiceInfo> list = new List<VideoOS.Platform.Util.ServiceInfo>();
		ServiceInfo[] array2 = array;
		foreach (ServiceInfo serviceInfo in array2)
		{
			list.Add(new VideoOS.Platform.Util.ServiceInfo
			{
				type = serviceInfo.type,
				description = serviceInfo.description,
				endpointDefinitions = serviceInfo.endpointdefinition,
				endpoints = new ArrayOfEndpointDefinition(serviceInfo.ServiceEndpoints),
				instance = serviceInfo.instance,
				name = serviceInfo.name,
				trusted = serviceInfo.trusted,
				uri = new ArrayOfString(serviceInfo.uri)
			});
		}
		return list.ToArray();
	}

	bool IServiceRegistration.SetServiceData(Guid instance, string serviceData)
	{
		return CallWithRetry((ServiceRegistrationServiceClient client) => client.SetServiceData(instance, serviceData));
	}

	void IServiceRegistration.UnregisterService(Guid instance)
	{
		CallWithRetryVoid(delegate
		{
			_serviceRegistrationService.UnregisterService(instance);
		});
	}

	bool IServiceRegistration.RegisterService(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData)
	{
		return CallWithRetry((ServiceRegistrationServiceClient client) => client.RegisterService(type, instance, uri.ToArray(), name, description, customData));
	}

	bool IServiceRegistration.RegisterServiceWithEndpoints(Guid type, Guid instance, ArrayOfString uri, string name, string description, string customData, string endpointDefinitions)
	{
		return CallWithRetry((ServiceRegistrationServiceClient client) => client.RegisterServiceWithEndpoints(type, instance, uri.ToArray(), name, description, customData, endpointDefinitions));
	}

	public void Dispose()
	{
		CloseConnection();
	}

	private void Connect()
	{
		Uri uri = new UriBuilder(Url).Uri;
		_serviceRegistrationService = GetClient(uri, Credentials, IsBasicUser);
		_serviceRegistrationService.InnerChannel.Open();
		long num = DateTime.Now.Ticks + 1000000;
		while (DateTime.Now.Ticks < num && _serviceRegistrationService.State == CommunicationState.Opening)
		{
			Thread.Sleep(5);
		}
	}

	public virtual ServiceRegistrationServiceClient GetClient(Uri url, ICredentials credentials, bool isBasicUser)
	{
		Binding binding = ServiceApiSvcHelper.GetBinding(credentials, url, isBasicUser);
		string spn = SpnFactory.GetSpn(url);
		EndpointAddress remoteAddress = new EndpointAddress(url, new SpnEndpointIdentity(spn));
		ServiceRegistrationServiceClient serviceRegistrationServiceClient = new ServiceRegistrationServiceClient(binding, remoteAddress);
		if (isBasicUser)
		{
			serviceRegistrationServiceClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
		}
		ServiceApiSvcHelper.SetCredentials(serviceRegistrationServiceClient.ClientCredentials, credentials, url, isBasicUser);
		return serviceRegistrationServiceClient;
	}

	public virtual void CloseConnection()
	{
		try
		{
			if (_serviceRegistrationService != null)
			{
				_serviceRegistrationService.Close();
			}
		}
		catch
		{
			_serviceRegistrationService.Abort();
		}
		_serviceRegistrationService = null;
	}

	private TResult CallWithRetry<TResult>(Func<ServiceRegistrationServiceClient, TResult> call)
	{
		try
		{
			if (_serviceRegistrationService == null)
			{
				Connect();
			}
			return call(_serviceRegistrationService);
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationException)
		{
			CloseConnection();
			Connect();
			return call(_serviceRegistrationService);
		}
		catch (ObjectDisposedException)
		{
			CloseConnection();
			Connect();
			return call(_serviceRegistrationService);
		}
	}

	private void CallWithRetryVoid(Action call)
	{
		try
		{
			if (_serviceRegistrationService == null)
			{
				Connect();
			}
			call();
		}
		catch (FaultException)
		{
			throw;
		}
		catch (CommunicationException)
		{
			CloseConnection();
			Connect();
			call();
		}
		catch (ObjectDisposedException)
		{
			CloseConnection();
			Connect();
			call();
		}
	}
}
