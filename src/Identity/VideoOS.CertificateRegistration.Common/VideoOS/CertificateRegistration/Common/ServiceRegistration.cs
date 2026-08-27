using System;
using System.Net;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.WebService.Common.ServiceRegistrationService;

namespace VideoOS.CertificateRegistration.Common;

public class ServiceRegistration : IServiceRegistration, IDisposable
{
	private const int IdpDiscoveryTimeout = 3;

	private IServiceRegistrationServiceProxy _implementation;

	private IdentityServerWindowsTokenHandler _tokenHandler;

	private ILogger _logger;

	private SimpleTokenCache _tokenCache;

	public ServiceRegistration(Uri managementServerAddress, Uri unsecureManagementServerAddress, Uri identityProvider, NetworkCredential credentials, ILogger logger)
	{
		_logger = logger;
		try
		{
			IdpDiscoveryType idpDiscoveryType = (identityProvider.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) ? IdpDiscoveryType.Secure : IdpDiscoveryType.Unsecure);
			IIdpServerInformation result = new IdpServerValidation(new HttpGet(), 3, idpDiscoveryType, null).ValidateIdpServerAsync(identityProvider).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			if (result.IsValid && result.IsOAuthSupported)
			{
				_tokenHandler = new IdentityServerWindowsTokenHandler(credentials, new IdentityServerClientSettings(identityProvider, "GrantValidatorClient"));
				_tokenCache = new SimpleTokenCache(_tokenHandler);
				_implementation = ServiceRegistrationServiceProxy.CreateNew(managementServerAddress.Host, managementServerAddress.Port, managementServerAddress.Scheme == Uri.UriSchemeHttps, _tokenCache);
			}
		}
		catch (Exception ex)
		{
			_logger?.LogError(FormattableString.Invariant($"Failed to create ServiceRegistrationServiceProxy with OAuth support, Exception = {ex}"));
		}
		if (_implementation == null)
		{
			_implementation = ((credentials == null) ? ServiceRegistrationServiceProxy.CreateNew(unsecureManagementServerAddress.Host, unsecureManagementServerAddress.Port) : ServiceRegistrationServiceProxy.CreateNew(unsecureManagementServerAddress.Host, unsecureManagementServerAddress.Port, credentials.UserName, credentials.Password, credentials.Domain));
			_logger?.LogInfo("No OAuth authentication available, fall back to legacy authentication (Windows authentication)");
		}
	}

	public void Dispose()
	{
		_implementation?.Dispose();
		_tokenCache?.Dispose();
		_tokenHandler?.Dispose();
	}

	public void EnableService(Guid instance, bool enabled)
	{
		_implementation.EnableService(instance, enabled);
	}

	public string GetServiceData(Guid instance)
	{
		return _implementation.GetServiceData(instance);
	}

	public ServiceInfo[] GetServices()
	{
		return _implementation.GetServices();
	}

	public ServiceInfo[] GetServicesIncludingDisabled()
	{
		return _implementation.GetServicesIncludingDisabled();
	}

	public bool RegisterService(Guid type, Guid instance, string[] uri, string name, string description, string customData)
	{
		return _implementation.RegisterService(type, instance, uri, name, description, customData);
	}

	public bool RegisterServiceWithEndpoints(Guid type, Guid instance, string[] uri, string name, string description, string customData, string endpointDefinition)
	{
		return _implementation.RegisterServiceWithEndpoints(type, instance, uri, name, description, customData, endpointDefinition);
	}

	public bool SetServiceData(Guid instance, string customData)
	{
		return _implementation.SetServiceData(instance, customData);
	}

	public void UnregisterService(Guid instance)
	{
		_implementation.UnregisterService(instance);
	}
}
