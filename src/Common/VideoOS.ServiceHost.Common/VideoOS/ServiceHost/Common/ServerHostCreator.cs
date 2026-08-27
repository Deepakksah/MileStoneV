using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServiceHost.Common;

public abstract class ServerHostCreator
{
	private static bool? _hasCertificate;

	protected IWebServiceUtils WebServiceUtils { get; }

	protected static bool HasCertificate
	{
		get
		{
			if (_hasCertificate.HasValue)
			{
				return _hasCertificate.Value;
			}
			return false;
		}
		private set
		{
			_hasCertificate = value;
		}
	}

	protected IWebServiceSettings WebServiceSettings { get; }

	protected virtual TimeSpan ReceiveTimeout => TimeSpan.FromMilliseconds(WebServiceSettings.ReceiveTimeout);

	protected virtual int MaxItemsInObjectGraph => 1000000;

	protected ServerHostCreator(ICertificateRepository certificateRepository, IWebServiceSettings webServiceSettings)
	{
		certificateRepository.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 27);
		webServiceSettings.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 28);
		if (!_hasCertificate.HasValue)
		{
			HasCertificate = certificateRepository.HasCertificate;
		}
		WebServiceSettings = webServiceSettings;
		WebServiceUtils = new WebServiceUtils(WebServiceSettings);
	}

	protected System.ServiceModel.ServiceHost CreateServiceHost(object serviceInstance, string relativeUrl = null)
	{
		serviceInstance.NullCheck("", "CreateServiceHost", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 47);
		try
		{
			Uri address = GetAddress(relativeUrl);
			System.ServiceModel.ServiceHost serviceHost = new System.ServiceModel.ServiceHost(serviceInstance, address);
			ServiceMetadataBehavior item = WebServiceUtils.CreateServiceMetadataBehavior(HasCertificate);
			serviceHost.Description.Behaviors.Add(item);
			ServiceThrottlingBehavior item2 = CreateServiceThrottlingBehavior();
			serviceHost.Description.Behaviors.Add(item2);
			return serviceHost;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			throw;
		}
	}

	protected System.ServiceModel.ServiceHost CreateServiceWithCredentials<T>(string relativeUrl, string spnName, T commandCredentialsInstance, bool enableGetForMetadataBehaviour)
	{
		relativeUrl.NullCheck("", "CreateServiceWithCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 82);
		spnName.NullCheck("", "CreateServiceWithCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 83);
		System.ServiceModel.ServiceHost serviceHost = CreateServiceHost(commandCredentialsInstance, relativeUrl);
		AddCredentialAuthenticatedHostEndpoint<T>(serviceHost, relativeUrl, spnName);
		return serviceHost;
	}

	protected System.ServiceModel.ServiceHost CreateServiceWithToken<T>(string relativeUrl, string spnName, T commandTokenInstance)
	{
		relativeUrl.NullCheck("", "CreateServiceWithToken", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 100);
		spnName.NullCheck("", "CreateServiceWithToken", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 101);
		System.ServiceModel.ServiceHost serviceHost = CreateServiceHost(commandTokenInstance, relativeUrl);
		AddTokenAuthenticatedHostEndpoint<T>(serviceHost, relativeUrl, spnName);
		return serviceHost;
	}

	protected System.ServiceModel.ServiceHost CreateServiceWithCertificateCredentials<T>(string relativeUrl, string spnName, T commandCredentialsInstance, ServiceCredentials serviceCredentials)
	{
		relativeUrl.NullCheck("", "CreateServiceWithCertificateCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 119);
		spnName.NullCheck("", "CreateServiceWithCertificateCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 120);
		System.ServiceModel.ServiceHost serviceHost = CreateServiceHost(commandCredentialsInstance, relativeUrl);
		AddHostEndpointForCertificateCredentials<T>(serviceHost, relativeUrl, spnName);
		serviceHost.Description.Behaviors.Add(serviceCredentials);
		return serviceHost;
	}

	protected void AddCredentialAuthenticatedHostEndpoint<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddCredentialAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 136);
		relativeUrl.NullCheck("", "AddCredentialAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 137);
		spnName.NullCheck("", "AddCredentialAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 138);
		if (HasCertificate)
		{
			AddHostEndpointForUserCredentialsAndCertificate<T>(host, relativeUrl, spnName);
		}
		else
		{
			AddHostEndpointForUserCredentialsAndNoCertificate<T>(host, relativeUrl, spnName);
		}
	}

	protected void AddTokenAuthenticatedHostEndpoint<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddTokenAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 159);
		relativeUrl.NullCheck("", "AddTokenAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 160);
		spnName.NullCheck("", "AddTokenAuthenticatedHostEndpoint", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 161);
		if (HasCertificate)
		{
			AddHostEndpointForTokenAndCertificate<T>(host, relativeUrl, spnName);
		}
		else
		{
			AddHostEndpointForTokenAndNoCertificate<T>(host, relativeUrl);
		}
	}

	private void AddHostEndpointForTokenAndNoCertificate<T>(System.ServiceModel.ServiceHost host, string relativeUrl)
	{
		host.NullCheck("", "AddHostEndpointForTokenAndNoCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 180);
		relativeUrl.NullCheck("", "AddHostEndpointForTokenAndNoCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 181);
		Uri address = GetAddress(relativeUrl);
		BasicHttpBinding binding = WebServiceUtils.CreateBasicHttpBinding(ReceiveTimeout);
		ServiceEndpoint maxItemsInObjectGraph = host.AddServiceEndpoint(typeof(T), binding, address);
		SetMaxItemsInObjectGraph(maxItemsInObjectGraph);
	}

	private void AddHostEndpointForTokenAndCertificate<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddHostEndpointForTokenAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 197);
		relativeUrl.NullCheck("", "AddHostEndpointForTokenAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 198);
		spnName.NullCheck("", "AddHostEndpointForTokenAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 199);
		Uri address = GetAddress(relativeUrl);
		BasicHttpBinding basicHttpBinding = WebServiceUtils.CreateBasicHttpBinding(ReceiveTimeout);
		basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
		ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(T), basicHttpBinding, address);
		serviceEndpoint.Address = new EndpointAddress(address, EndpointIdentity.CreateSpnIdentity(spnName));
		SetMaxItemsInObjectGraph(serviceEndpoint);
	}

	private void AddHostEndpointForUserCredentialsAndNoCertificate<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddHostEndpointForUserCredentialsAndNoCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 218);
		relativeUrl.NullCheck("", "AddHostEndpointForUserCredentialsAndNoCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 219);
		spnName.NullCheck("", "AddHostEndpointForUserCredentialsAndNoCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 220);
		Uri address = GetAddress(relativeUrl);
		WSHttpBinding binding = WebServiceUtils.CreateWSHttpBinding(SecurityMode.Message, ReceiveTimeout, WSMessageEncoding.Text);
		ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(T), binding, address);
		serviceEndpoint.Address = new EndpointAddress(address, EndpointIdentity.CreateSpnIdentity(spnName));
		SetMaxItemsInObjectGraph(serviceEndpoint);
	}

	private void AddHostEndpointForUserCredentialsAndCertificate<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddHostEndpointForUserCredentialsAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 237);
		relativeUrl.NullCheck("", "AddHostEndpointForUserCredentialsAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 238);
		spnName.NullCheck("", "AddHostEndpointForUserCredentialsAndCertificate", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 239);
		Uri address = GetAddress(relativeUrl);
		WSHttpBinding wSHttpBinding = WebServiceUtils.CreateWSHttpBinding(SecurityMode.Message, ReceiveTimeout, WSMessageEncoding.Text);
		wSHttpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
		ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(T), wSHttpBinding, address);
		serviceEndpoint.Address = new EndpointAddress(address, EndpointIdentity.CreateSpnIdentity(spnName));
		SetMaxItemsInObjectGraph(serviceEndpoint);
	}

	private void AddHostEndpointForCertificateCredentials<T>(System.ServiceModel.ServiceHost host, string relativeUrl, string spnName)
	{
		host.NullCheck("", "AddHostEndpointForCertificateCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 257);
		relativeUrl.NullCheck("", "AddHostEndpointForCertificateCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 258);
		spnName.NullCheck("", "AddHostEndpointForCertificateCredentials", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 259);
		Uri address = GetAddress(relativeUrl);
		SecurityMode securityMode = (HasCertificate ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message);
		WSHttpBinding binding = WebServiceUtils.CreateWSHttpBinding(securityMode, ReceiveTimeout, MessageCredentialType.Certificate);
		ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(T), binding, address);
		serviceEndpoint.Address = new EndpointAddress(address, EndpointIdentity.CreateSpnIdentity(spnName));
		SetMaxItemsInObjectGraph(serviceEndpoint);
	}

	private Uri GetAddress(string relativeUrl = null)
	{
		UriBuilder uriBuilder = new UriBuilder(HasCertificate ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, Environment.MachineName, WebServiceSettings.WebServicePort);
		if (!string.IsNullOrEmpty(relativeUrl))
		{
			uriBuilder.Path = relativeUrl;
		}
		return uriBuilder.Uri;
	}

	protected virtual ServiceThrottlingBehavior CreateServiceThrottlingBehavior()
	{
		return WebServiceUtils.CreateServiceThrottlingBehavior();
	}

	protected void SetMaxItemsInObjectGraph(ServiceHostBase host)
	{
		host.NullCheck("", "SetMaxItemsInObjectGraph", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 286);
		foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
		{
			SetMaxItemsInObjectGraph(endpoint);
		}
	}

	private void SetMaxItemsInObjectGraph(ServiceEndpoint endpoint)
	{
		endpoint.NullCheck("", "SetMaxItemsInObjectGraph", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.Common\\ServerHostCreator.cs", 295);
		foreach (OperationDescription operation in endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = MaxItemsInObjectGraph;
			}
		}
	}
}
