using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.ServerConfigurator.RegistrationProxy;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class ClientRegistration : RegistrationBase<ClientEndpointConfiguration, IClientRegistrationV2, RegisterClientStatus>
{
	private readonly NetworkCredential _credentials;

	private readonly Uri _authorizationServer;

	protected override IRegistrationActivator RegistrationActivator => ProxyActivator.Instance;

	public ClientRegistration(IEnumerable<ClientEndpointConfiguration> clientEndpoints, NetworkCredential credentials, Uri authorizationServer, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration)
		: base(clientEndpoints, registrationLogger, logger, innerRegistration)
	{
		_credentials = credentials;
		_authorizationServer = authorizationServer;
	}

	protected override ImplementationDefinition ImplementationDefinition(ClientEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		return endpointConfiguration.ClientRegistrar;
	}

	protected override StatusCode ExecuteInitialize(IClientRegistrationV2 target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(IClientRegistrationV2 target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreRegisterClient();
	}

	protected override Status Execute(IClientRegistrationV2 target, ClientEndpointConfiguration endpoint)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.RegisterClient(_credentials, _authorizationServer);
	}

	protected override Status PostExecute(IClientRegistrationV2 target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostRegisterClient();
	}

	protected override bool IsSuccess(RegisterClientStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override RegisterClientStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return RegisterClientStatusFromStatusCode(statusCode);
	}

	private static RegisterClientStatus RegisterClientStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => RegisterClientStatus.Success, 
			StatusCode.VmsAdminCredentialsNeeded => RegisterClientStatus.InsufficientCredentials, 
			StatusCode.AuthorizationServerNotFound => RegisterClientStatus.AuthorizationServerNotFound, 
			StatusCode.UnspecifiedErrorRegistration => RegisterClientStatus.UnspecifiedError, 
			StatusCode.UnhandledException => RegisterClientStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => RegisterClientStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => RegisterClientStatus.CouldNotStopServer, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	public static void Cleanup(ClientEndpointConfiguration clientEndpoint, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (IClientCleanup value in RegisterClientCleanUpProxy.GetEndpointInterfaces(new ClientEndpointConfiguration[1] { clientEndpoint }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
