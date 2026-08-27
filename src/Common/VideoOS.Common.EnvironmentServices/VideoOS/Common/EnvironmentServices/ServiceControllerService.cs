using System;
using System.Linq;
using System.ServiceProcess;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
public class ServiceControllerService : IServiceControllerService, IDisposable
{
	private ServiceController _serviceController;

	private bool _disposed;

	private IServiceControllerService[] _dependentServices;

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

	public ServiceControllerStatus Status => _serviceController.Status;

	public IServiceControllerService[] DependentServices
	{
		get
		{
			IServiceControllerService[] array = _dependentServices;
			if (array == null)
			{
				IServiceControllerService[] dependentServices = _serviceController.DependentServices.Select((ServiceController x) => new ServiceControllerService(x)).ToArray();
				array = (_dependentServices = dependentServices);
			}
			return array;
		}
	}

	public bool CanStop => _serviceController.CanStop;

	public ServiceControllerService(string serviceName)
	{
		_serviceController = new ServiceController(serviceName);
	}

	public ServiceControllerService(ServiceController serviceController)
	{
		_serviceController = serviceController;
	}

	public void Start()
	{
		_serviceController.Start();
	}

	public void Stop()
	{
		_serviceController.Stop();
	}

	public void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout)
	{
		_serviceController.WaitForStatus(desiredStatus, timeout);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing && !_disposed && _serviceController != null)
		{
			_serviceController.Dispose();
		}
		_disposed = true;
	}
}
