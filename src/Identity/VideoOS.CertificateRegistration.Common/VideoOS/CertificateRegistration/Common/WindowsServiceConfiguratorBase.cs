using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

public abstract class WindowsServiceConfiguratorBase : MarshalByRefObject
{
	private readonly string _serviceName;

	private ServiceControllerStatus _originalServerStatus;

	private readonly TimeSpan _timeout;

	private IServiceControllerFactory _serviceControllerFactory;

	private const int ERROR_SERVICE_ALREADY_RUNNING = 1056;

	private const int ERROR_SERVICE_NOT_ACTIVE = 1062;

	protected ILogger Logger { get; private set; }

	protected WindowsServiceConfiguratorBase(string serviceName, TimeSpan timeout, IServiceControllerFactory serviceControllerFactory)
	{
		_serviceName = serviceName;
		_timeout = timeout;
		_serviceControllerFactory = ((_serviceName != null) ? serviceControllerFactory : null);
	}

	public Status Initialize(ILogger logger)
	{
		Logger = logger ?? throw new ArgumentNullException("logger");
		return ExecuteInitialize();
	}

	protected virtual Status ExecuteInitialize()
	{
		return Status.SuccessStatus();
	}

	public virtual Status PreExecute()
	{
		switch (GetServiceStatus())
		{
		case ServiceControllerStatus.StartPending:
		case ServiceControllerStatus.Running:
		case ServiceControllerStatus.ContinuePending:
		case ServiceControllerStatus.PausePending:
		case ServiceControllerStatus.Paused:
			_originalServerStatus = ServiceControllerStatus.Running;
			StopService();
			break;
		}
		if (!WaitForServiceStatus(ServiceControllerStatus.Stopped))
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.CouldNotStopServer), FormattableString.Invariant($"Could not stop service {_serviceName}"), "PreExecute");
		}
		return Status.SuccessStatus();
	}

	public virtual Status PostExecute()
	{
		if (_originalServerStatus == ServiceControllerStatus.Running)
		{
			return StartServiceAndWait();
		}
		return Status.SuccessStatus();
	}

	protected Status StartServiceAndWait()
	{
		StartService();
		if (!WaitForServiceStatus(ServiceControllerStatus.Running))
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.CouldNotStartServer), FormattableString.Invariant($"Could not start service {_serviceName}"), "StartServiceAndWait");
		}
		return Status.SuccessStatus();
	}

	protected Status LoggedErrorStatus(Exception exception, StatusCode code, string description = null, [CallerMemberName] string action = "")
	{
		if (exception == null)
		{
			throw new ArgumentNullException("exception");
		}
		string text = FormattableString.Invariant($"Error occurred in {action}: {description} {exception.Message}");
		Logger?.LogError(FormattableString.Invariant($"{text}{Environment.NewLine}{exception.ToString()}"));
		return Status.ErrorStatus(code);
	}

	protected Status LoggedStatus(Status status, string description = null, [CallerMemberName] string action = "")
	{
		if (status == null)
		{
			throw new ArgumentNullException("status");
		}
		if (Logger != null)
		{
			string entry = FormattableString.Invariant($"'{GetType().Name}.{action}' Status = {status.StatusCode} {((description != null) ? FormattableString.Invariant($" - Additional information: {description}") : string.Empty)}");
			if (status.StatusCode == StatusCode.Success)
			{
				Logger.LogInfo(entry);
			}
			else
			{
				Logger.LogError(entry);
			}
		}
		return status;
	}

	protected Status LoggedIfSuccessOrWarning(Status status, [CallerMemberName] string action = "")
	{
		if (status == null)
		{
			throw new ArgumentNullException("status");
		}
		if (Logger != null)
		{
			string entry = FormattableString.Invariant($"'{GetType().Name}.{action}' Status = {status.StatusCode}");
			if (status.StatusCode == StatusCode.Success)
			{
				Logger.LogInfo(entry);
			}
			else if (status.StatusCode == StatusCode.ServiceRegistrationNotFound)
			{
				Logger.LogWarning(entry);
			}
		}
		return status;
	}

	private void StartService()
	{
		try
		{
			using IServiceControllerService serviceControllerService = _serviceControllerFactory?.Create(_serviceName);
			serviceControllerService?.Start();
		}
		catch (Exception ex)
		{
			if (ex.GetBaseException() is Win32Exception { NativeErrorCode: 1056 })
			{
				RestartService();
				return;
			}
			throw new ServiceConfigurationException(FormattableString.Invariant($"Unable to start the service {_serviceName}."), ex);
		}
	}

	private void RestartService()
	{
		try
		{
			using IServiceControllerService serviceControllerService = _serviceControllerFactory.Create(_serviceName);
			if (serviceControllerService.Status.Equals(ServiceControllerStatus.Running) || serviceControllerService.Status.Equals(ServiceControllerStatus.StartPending))
			{
				serviceControllerService.Stop();
			}
			serviceControllerService.WaitForStatus(ServiceControllerStatus.Stopped, _timeout);
			serviceControllerService.Start();
			serviceControllerService.WaitForStatus(ServiceControllerStatus.Running, _timeout);
		}
		catch (Exception innerException)
		{
			throw new ServiceConfigurationException(FormattableString.Invariant($"Unable to restart the service {_serviceName}."), innerException);
		}
	}

	private void StopService()
	{
		try
		{
			using IServiceControllerService serviceControllerService = _serviceControllerFactory?.Create(_serviceName);
			serviceControllerService?.Stop();
		}
		catch (Exception ex)
		{
			if (ex.GetBaseException() is Win32Exception { NativeErrorCode: 1062 })
			{
				return;
			}
			throw new ServiceConfigurationException(FormattableString.Invariant($"Unable to stop the service {_serviceName}."), ex);
		}
	}

	private ServiceControllerStatus GetServiceStatus()
	{
		try
		{
			using IServiceControllerService serviceControllerService = _serviceControllerFactory?.Create(_serviceName);
			return serviceControllerService?.Status ?? ServiceControllerStatus.Stopped;
		}
		catch (Exception innerException)
		{
			throw new ServiceConfigurationException(FormattableString.Invariant($"Unable to get the status of the service {_serviceName}."), innerException);
		}
	}

	private bool WaitForServiceStatus(ServiceControllerStatus newStatus)
	{
		using IServiceControllerService serviceControllerService = _serviceControllerFactory?.Create(_serviceName);
		try
		{
			serviceControllerService?.WaitForStatus(newStatus, _timeout);
			return true;
		}
		catch (System.ServiceProcess.TimeoutException)
		{
			return false;
		}
	}
}
