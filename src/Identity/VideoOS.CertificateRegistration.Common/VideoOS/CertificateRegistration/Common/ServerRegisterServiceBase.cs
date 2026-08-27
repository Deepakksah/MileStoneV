using System;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Security;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.CertificateRegistration.Common;

public abstract class ServerRegisterServiceBase : WindowsServiceConfiguratorBase, IRegisterServiceV3, IServerCleanup
{
	private const int WAIT_FOR_SERVICE_TIMEOUT = 180;

	private readonly string _registeredServiceName;

	private readonly Guid _serviceType;

	private readonly IDnsService _dnsService;

	protected readonly IServiceRegistrationFactory ServiceRegistrationServiceFactory;

	private string _hostToRegister;

	protected ServerRegisterServiceBase(IDnsService dnsService, string serviceName, string registeredServiceName, Guid serviceType)
		: this(new ServiceRegistrationFactory(), dnsService, serviceName, registeredServiceName, serviceType, handleServiceStartStop: true)
	{
	}

	protected ServerRegisterServiceBase(IServiceRegistrationFactory serviceRegistrationServiceFactory, IDnsService dnsService, string serviceName, string registeredServiceName, Guid serviceType, bool handleServiceStartStop)
		: base(handleServiceStartStop ? serviceName : null, new TimeSpan(0, 0, 180), new ServiceControllerFactory())
	{
		ServiceRegistrationServiceFactory = serviceRegistrationServiceFactory ?? throw new ArgumentNullException("serviceRegistrationServiceFactory");
		if (handleServiceStartStop && string.IsNullOrEmpty(serviceName))
		{
			throw new ArgumentNullException("serviceName");
		}
		if (string.IsNullOrEmpty(registeredServiceName))
		{
			throw new ArgumentNullException("registeredServiceName");
		}
		if (serviceType == default(Guid))
		{
			throw new ArgumentException("serviceType");
		}
		_registeredServiceName = registeredServiceName;
		_serviceType = serviceType;
		_dnsService = dnsService ?? throw new ArgumentNullException("dnsService");
	}

	public Status CleanUp()
	{
		AdditionalCleanup();
		try
		{
			return LoggedIfSuccessOrWarning(ExecuteCleanup(), "CleanUp");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "CleanUp");
		}
	}

	public Status PostRegisterService()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PostExecute(), "PostRegisterService");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PostRegisterService");
		}
	}

	public Status PreRegisterService()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreRegisterService");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PreRegisterService");
		}
	}

	public Status RegisterThisService(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, string hostToRegister, bool isSecure, Credential credentials)
	{
		if (serverToRegisterTo == null)
		{
			throw new ArgumentNullException("serverToRegisterTo");
		}
		if (string.IsNullOrEmpty(hostToRegister))
		{
			throw new ArgumentNullException("hostToRegister");
		}
		_hostToRegister = hostToRegister;
		try
		{
			return LoggedIfSuccessOrWarning(Execute(serverToRegisterTo, unsecureServerToRegisterTo, identityProvider, isSecure, credentials), "RegisterThisService");
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			bool handled;
			Status status = ResolveException(ex, out handled, "RegisterThisService");
			return LoggedErrorStatus(ex, status.StatusCode, null, "RegisterThisService");
		}
	}

	protected virtual ServiceRegistrationParameters UpdateRegistrationParameters(ServiceRegistrationParameters registrationParamters, bool isSecure)
	{
		if (registrationParamters == null)
		{
			return CreateRegistrationParameters(isSecure);
		}
		if (registrationParamters.MainUri == null)
		{
			throw new InvalidOperationException(FormattableString.Invariant($"The Service with Id = {_serviceType} ({_registeredServiceName}) was registered with an invalid URI"));
		}
		registrationParamters.MainUri = UpdateUri(registrationParamters.MainUri, isSecure);
		return registrationParamters;
	}

	protected virtual Uri UpdateUri(Uri currentUri, bool isSecure)
	{
		return new UriBuilder(currentUri.Scheme, _hostToRegister, currentUri.Port, currentUri.AbsolutePath).Uri;
	}

	protected virtual ServiceRegistrationParameters CreateRegistrationParameters(bool isSecure)
	{
		throw new InvalidOperationException(FormattableString.Invariant($"The Service with Id = {_serviceType} ({_registeredServiceName}) was not registered, and thus cannot be reregistered"));
	}

	protected virtual Status Execute(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, bool isSecure, NetworkCredential networkCredential)
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"About to register service: \"{_registeredServiceName}\""));
		using IServiceRegistration serviceRegistration = ServiceRegistrationServiceFactory.Create(serverToRegisterTo, unsecureServerToRegisterTo, identityProvider, networkCredential, base.Logger);
		ServiceInfo serviceInfo = GetServiceInfo(serviceRegistration);
		if (serviceInfo != null)
		{
			base.Logger?.LogInfo(FormattableString.Invariant($"Found service already registered, about to unregister old service: \"{_registeredServiceName}\""));
			string serviceData = serviceRegistration.GetServiceData(serviceInfo.instance);
			ServiceRegistrationParameters registrationParamters = new ServiceRegistrationParameters(serviceInfo, serviceData);
			registrationParamters = UpdateRegistrationParameters(registrationParamters, isSecure);
			serviceRegistration.RegisterService(_serviceType, serviceInfo.instance, registrationParamters.Uris, registrationParamters.Name, registrationParamters.Description, registrationParamters.ServiceData);
			base.Logger?.LogInfo(FormattableString.Invariant($"Service registration finished"));
			return Status.SuccessStatus();
		}
		base.Logger?.LogWarning(FormattableString.Invariant($"Service registration skipped"));
		return Status.ErrorStatus(StatusCode.ServiceRegistrationNotFound);
	}

	protected virtual ServiceInfo GetServiceInfo(IServiceRegistration serviceRegistrationServiceClient)
	{
		return serviceRegistrationServiceClient.GetServices().FirstOrDefault((ServiceInfo x) => x.type == _serviceType);
	}

	private Status ExecuteCleanup()
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"About to ExecuteCleanup"));
		base.Logger?.LogInfo(FormattableString.Invariant($"ExecuteCleanup has been done."));
		return Status.SuccessStatus();
	}

	protected virtual void AdditionalCleanup()
	{
	}

	private Status ResolveException(Exception ex, out bool handled, [CallerMemberName] string action = "")
	{
		handled = true;
		if (ex is AggregateException)
		{
			if (ex.InnerException != null)
			{
				return ResolveException(ex.InnerException, out handled, "ResolveException");
			}
			return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "Service registration failed, Exception = " + ex, action);
		}
		if (ex is UriFormatException)
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.AuthorizationServerAddressInvalid), "HostnameIsInvalid, Exception = " + ex, action);
		}
		if (ex is SecurityNegotiationException || (ex is FaultException && ex.Message.ToUpperInvariant().Contains("VMO61008")))
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.VmsAdminCredentialsNeeded), "The caller was not authenticated by the service, Exception = " + ex, action);
		}
		if (ex.InnerException != null && ex.InnerException is UnauthorizedException)
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.VmsAdminCredentialsNeeded), "The caller was not authenticated by the service, Exception = " + ex, action);
		}
		handled = false;
		return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "Service registration failed, Exception = " + ex, action);
	}
}
