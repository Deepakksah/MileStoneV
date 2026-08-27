using System;
using System.ServiceProcess;

namespace VideoOS.Mobile.Helpers;

public class WindowsServiceControllerWrapper : IServiceController, IDisposable
{
	private ServiceController _serviceController;

	public ServiceControllerStatus Status => _serviceController.Status;

	public string ServiceName
	{
		get
		{
			return _serviceController.ServiceName;
		}
		set
		{
			_serviceController.ServiceName = value;
		}
	}

	public string MachineName
	{
		get
		{
			return _serviceController.MachineName;
		}
		set
		{
			_serviceController.MachineName = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return _serviceController.DisplayName;
		}
		set
		{
			_serviceController.DisplayName = value;
		}
	}

	public bool CanStop => _serviceController.CanStop;

	public bool CanShutdown => _serviceController.CanShutdown;

	public bool CanPauseAndContinue => _serviceController.CanPauseAndContinue;

	public ServiceType ServiceType => _serviceController.ServiceType;

	public ServiceStartMode StartType => _serviceController.StartType;

	public WindowsServiceControllerWrapper(ServiceController serviceController)
	{
		_serviceController = serviceController;
	}

	public WindowsServiceControllerWrapper(string serviceName)
	{
		_serviceController = new ServiceController(serviceName);
	}

	public void Close()
	{
		_serviceController.Close();
	}

	public void Continue()
	{
		_serviceController.Continue();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_serviceController.Dispose();
		}
	}

	public void Pause()
	{
		_serviceController.Pause();
	}

	public void Start()
	{
		_serviceController.Start();
	}

	public void Start(string[] args)
	{
		_serviceController.Start(args);
	}

	public void Stop()
	{
		_serviceController.Stop();
	}

	public void WaitForStatus(ServiceControllerStatus desiredStatus)
	{
		_serviceController.WaitForStatus(desiredStatus);
	}

	public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
	{
		_serviceController.WaitForStatus(desiredStatus, timeout);
	}
}
