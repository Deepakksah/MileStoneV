using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SetServerAddressHandler : RegistrationBase<ClientEndpointConfiguration, ISetServerAddress, SetServerAddressStatus>
{
	private readonly Uri _newUnsecureServerAddress;

	private readonly Uri _newSecureServerAddress;

	private readonly bool _preferredSecureState;

	private readonly IEnumerable<CertificateGroup> _certificateGroups;

	public SetServerAddressHandler(IEnumerable<ClientEndpointConfiguration> clientEndpoints, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration, Uri newUnsecureServerAddress, Uri newSecureServerAddress, bool preferredSecureState, IEnumerable<CertificateGroup> certificateGroups)
		: base(clientEndpoints, registrationLogger, logger, innerRegistration)
	{
		_newUnsecureServerAddress = newUnsecureServerAddress;
		_newSecureServerAddress = newSecureServerAddress;
		_preferredSecureState = preferredSecureState;
		_certificateGroups = certificateGroups;
	}

	protected override ImplementationDefinition ImplementationDefinition(ClientEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		return endpointConfiguration.ServerAddressRegistrar;
	}

	protected override StatusCode ExecuteInitialize(ISetServerAddress target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(ISetServerAddress target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreSetServerAddress();
	}

	protected override Status Execute(ISetServerAddress target, ClientEndpointConfiguration endpoint)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (endpoint == null)
		{
			throw new ArgumentNullException("endpoint");
		}
		bool flag = _certificateGroups.FirstOrDefault((CertificateGroup x) => x.Id == endpoint.ServerGroupDependency)?.UseEncryption ?? _preferredSecureState;
		if (endpoint.Id == new Guid("29e4156b-50e1-431d-8e18-b5bda88cb719") && endpoint.DisplayName == "EventServerClientEndPointDisplayName")
		{
			flag = false;
		}
		if (endpoint.Id == new Guid("943FAEF0-0378-4A85-A3DF-AF7D4AC987F3") && endpoint.DisplayName == "LogServerClientEndPointDisplayName")
		{
			flag = false;
		}
		return target.SetServerAddress(flag ? _newSecureServerAddress : _newUnsecureServerAddress);
	}

	protected override Status PostExecute(ISetServerAddress target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostSetServerAddress();
	}

	protected override bool IsSuccess(SetServerAddressStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override SetServerAddressStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return SetServerAddressStatusFromStatusCode(statusCode);
	}

	private static SetServerAddressStatus SetServerAddressStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => SetServerAddressStatus.Success, 
			StatusCode.UnspecifiedErrorRegistration => SetServerAddressStatus.UnspecifiedError, 
			StatusCode.UnhandledException => SetServerAddressStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => SetServerAddressStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => SetServerAddressStatus.CouldNotStopServer, 
			StatusCode.VmsAdminCredentialsNeeded => SetServerAddressStatus.InsufficientCredentials, 
			StatusCode.ManagementServerNotFound => SetServerAddressStatus.ManagementServerNotFound, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	public static void Cleanup(ClientEndpointConfiguration clientEndpoint, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (IClientCleanup value in SetServerAddressCleanUpProxy.GetEndpointInterfaces(new ClientEndpointConfiguration[1] { clientEndpoint }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
