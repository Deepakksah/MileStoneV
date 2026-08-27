using System;
using System.Runtime.CompilerServices;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

public abstract class SqlConnectionRegistrarBase : WindowsServiceConfiguratorBase, IRegisterSqlServer, IServerCleanup
{
	public SqlConnectionRegistrarBase(string serviceName, TimeSpan timeout)
		: this(serviceName, new TimeSpan(0, 3, 0), handleServiceStartStop: true)
	{
	}

	public SqlConnectionRegistrarBase(string serviceName, TimeSpan timeout, bool handleServiceStartStop)
		: base(handleServiceStartStop ? serviceName : null, timeout, new ServiceControllerFactory())
	{
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

	public Status PostRegisterSqlServer()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PostExecute(), "PostRegisterSqlServer");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PostRegisterSqlServer");
		}
	}

	public Status PreRegisterSqlServer()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreRegisterSqlServer");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PreRegisterSqlServer");
		}
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

	public Status SetSqlServerParameters(ConnectionStringParameters connectionStringParameters)
	{
		if (connectionStringParameters == null)
		{
			throw new ArgumentNullException("connectionStringParameters");
		}
		try
		{
			return LoggedIfSuccessOrWarning(ExecuteSetSqlServerParameters(connectionStringParameters), "SetSqlServerParameters");
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			bool handled;
			Status status = ResolveException(ex, out handled, "SetSqlServerParameters");
			return LoggedErrorStatus(ex, status.StatusCode, null, "SetSqlServerParameters");
		}
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
			return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "Setting SQL server parameters failed with AggregateException, Exception = " + ex, action);
		}
		handled = false;
		return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "Setting SQL server parameters failed, Exception = " + ex, action);
	}

	[Obsolete("Do not use, for future use", false)]
	protected abstract Status ExecuteSetSqlServerParameters(ConnectionStringParameters connectionStringParameters);
}
