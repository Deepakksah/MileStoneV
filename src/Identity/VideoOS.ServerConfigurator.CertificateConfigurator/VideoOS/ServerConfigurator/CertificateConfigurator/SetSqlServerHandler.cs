using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class SetSqlServerHandler : RegistrationBase<ServerEndpointConfiguration, IRegisterSqlServer, SetSqlServerStatus>
{
	private readonly ConnectionStringParameters _sqlParameters;

	public SetSqlServerHandler(IEnumerable<ServerEndpointConfiguration> serverEndpoints, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration, ConnectionStringParameters sqlParameters)
		: base(serverEndpoints, registrationLogger, logger, innerRegistration)
	{
		_sqlParameters = sqlParameters;
	}

	protected override ImplementationDefinition ImplementationDefinition(ServerEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		return endpointConfiguration.SqlServerRegistrar;
	}

	protected override StatusCode ExecuteInitialize(IRegisterSqlServer target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(IRegisterSqlServer target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreRegisterSqlServer();
	}

	protected override Status Execute(IRegisterSqlServer target, ServerEndpointConfiguration endpoint)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.SetSqlServerParameters(_sqlParameters);
	}

	protected override Status PostExecute(IRegisterSqlServer target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostRegisterSqlServer();
	}

	protected override bool IsSuccess(SetSqlServerStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override SetSqlServerStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return statusCode switch
		{
			StatusCode.Success => SetSqlServerStatus.Success, 
			StatusCode.UnspecifiedErrorRegistration => SetSqlServerStatus.UnspecifiedError, 
			StatusCode.UnhandledException => SetSqlServerStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => SetSqlServerStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => SetSqlServerStatus.CouldNotStopServer, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {statusCode}")), 
		};
	}

	public static void Cleanup(ServerEndpointConfiguration serverEndpoint, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (IServerCleanup value in SetSqlServerCleanUpProxy.GetEndpointInterfaces(new ServerEndpointConfiguration[1] { serverEndpoint }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
