using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SystemConfigurationHandler : RegistrationBase<SystemConfigurationElement, ISystemConfiguration, SystemConfigurationStatus>
{
	private readonly string _oldhostName;

	private readonly Uri _managementServerConfiguration;

	private readonly Uri _managementServerCommunication;

	private readonly Credential _credentials;

	private readonly Uri _authorizationAddress;

	public SystemConfigurationHandler(IEnumerable<SystemConfigurationElement> configurationElements, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration, string oldHostName, Uri managementServerConfiguration, Uri managementServerCommunication, Uri authorizationAddress, NetworkCredential credentials)
		: base(configurationElements, registrationLogger, logger, innerRegistration)
	{
		_oldhostName = oldHostName;
		_managementServerConfiguration = managementServerConfiguration;
		_managementServerCommunication = managementServerCommunication;
		_authorizationAddress = authorizationAddress;
		_credentials = credentials;
	}

	protected override ImplementationDefinition ImplementationDefinition(SystemConfigurationElement configurationElement)
	{
		if (configurationElement == null)
		{
			throw new ArgumentNullException("configurationElement");
		}
		return configurationElement.ConfigurationImplementation;
	}

	protected override StatusCode ExecuteInitialize(ISystemConfiguration target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(ISystemConfiguration target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreConfigureSystem();
	}

	protected override Status Execute(ISystemConfiguration target, SystemConfigurationElement endpoint)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.ConfigureSystem(_oldhostName, _managementServerConfiguration, _managementServerCommunication, _authorizationAddress, _credentials);
	}

	protected override Status PostExecute(ISystemConfiguration target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostConfigureSystem();
	}

	protected override bool IsSuccess(SystemConfigurationStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override SystemConfigurationStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return SystemConfigurationStatusFromStatusCode(statusCode);
	}

	private static SystemConfigurationStatus SystemConfigurationStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => SystemConfigurationStatus.Success, 
			StatusCode.UnspecifiedErrorRegistration => SystemConfigurationStatus.UnspecifiedError, 
			StatusCode.VmsAdminCredentialsNeeded => SystemConfigurationStatus.InsufficientCredentials, 
			StatusCode.AuthorizationServerNotFound => SystemConfigurationStatus.AuthorizationServerNotFound, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	public static void Cleanup(SystemConfigurationElement configurationElement, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (ISystemConfigurationCleanUp value in SystemConfigurationCleanupProxy.GetEndpointInterfaces(new SystemConfigurationElement[1] { configurationElement }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
