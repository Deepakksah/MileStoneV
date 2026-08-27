using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.ServerConfigurator.RegistrationProxy;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class CertificateRegistration : RegistrationBase<ServerEndpointConfiguration, IApplyCertificate, ApplyStatus>
{
	private readonly IDictionary<Guid, ModifiableCertificateGroup> _certificateGroups;

	protected override IRegistrationActivator RegistrationActivator => ProxyActivator.Instance;

	public CertificateRegistration(IEnumerable<ServerEndpointConfiguration> serverEndpoints, IDictionary<Guid, ModifiableCertificateGroup> certificateGroups, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration)
		: base(serverEndpoints, registrationLogger, logger, innerRegistration)
	{
		_certificateGroups = certificateGroups;
	}

	protected override ImplementationDefinition ImplementationDefinition(ServerEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		return endpointConfiguration.CertificateRegistrar;
	}

	protected override StatusCode ExecuteInitialize(IApplyCertificate target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(IApplyCertificate target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreApplyCertificate();
	}

	protected override Status Execute(IApplyCertificate target, ServerEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		string thumbprint = ((!_certificateGroups[endpointConfiguration.CertificateGroupId].UseEncryption) ? null : _certificateGroups[endpointConfiguration.CertificateGroupId].SelectedCertificate?.Thumbprint);
		return target.ApplyCertificate(thumbprint);
	}

	protected override Status PostExecute(IApplyCertificate target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostApplyCertificate();
	}

	protected override bool IsSuccess(ApplyStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override ApplyStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return ApplyStatusFromStatusCode(statusCode);
	}

	private static ApplyStatus ApplyStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => ApplyStatus.Success, 
			StatusCode.UnspecifiedErrorEncryption => ApplyStatus.UnspecifiedError, 
			StatusCode.UnhandledException => ApplyStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => ApplyStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => ApplyStatus.CouldNotStopServer, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	public static void Cleanup(ServerEndpointConfiguration serverEndpoint, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (IServerCleanup value in ApplyCertificateCleanUpProxy.GetEndpointInterfaces(new ServerEndpointConfiguration[1] { serverEndpoint }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
