using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Server.Admin.Utilities;

public sealed class IisControlledResetHelper : IDisposable
{
	private class ServiceWithDependencies
	{
		public IServiceControllerService ServiceController { get; }

		public IEnumerable<ServiceWithDependencies> Dependencies { get; set; }

		public ServiceWithDependencies(IServiceControllerService serviceController)
		{
			ServiceController = serviceController;
		}
	}

	private readonly string _serviceName;

	private readonly IServiceControllerFactory _serviceControllerFactory;

	private readonly ILogger _logger;

	private ServiceWithDependencies _serviceWithDependencies;

	public IisControlledResetHelper(string serviceName, ILogger logger)
		: this(serviceName, new ServiceControllerFactory(), logger)
	{
	}

	public IisControlledResetHelper(string serviceName, IServiceControllerFactory serviceControllerFactory, ILogger logger)
	{
		_serviceName = serviceName ?? throw new ArgumentNullException("serviceName");
		_serviceControllerFactory = serviceControllerFactory ?? throw new ArgumentNullException("serviceControllerFactory");
		_logger = logger ?? throw new ArgumentNullException("logger");
		_serviceWithDependencies = new ServiceWithDependencies(_serviceControllerFactory.Create(_serviceName));
		BuildDependencyTree(_serviceWithDependencies);
	}

	private void BuildDependencyTree(ServiceWithDependencies serviceWithDependencies)
	{
		IServiceControllerService serviceController = serviceWithDependencies.ServiceController;
		serviceWithDependencies.Dependencies = (from a in serviceController.DependentServices
			where a.Status == ServiceControllerStatus.Running || a.Status == ServiceControllerStatus.StartPending
			select new ServiceWithDependencies(a)).ToList();
		foreach (ServiceWithDependencies dependency in serviceWithDependencies.Dependencies)
		{
			BuildDependencyTree(dependency);
		}
	}

	public void StopServiceWithDependencies(TimeSpan timeout)
	{
		StopServices(_serviceWithDependencies, timeout);
	}

	public void ResumeServiceWithDependencies(TimeSpan timeout)
	{
		if (_serviceWithDependencies == null)
		{
			throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("Cannot resume service. call {0} first", "StopServiceWithDependencies")));
		}
		ResumeServices(_serviceWithDependencies, timeout);
	}

	private void StopServices(ServiceWithDependencies serviceWithDependencies, TimeSpan timeout)
	{
		IServiceControllerService serviceController = serviceWithDependencies.ServiceController;
		foreach (ServiceWithDependencies dependency in serviceWithDependencies.Dependencies)
		{
			StopServices(dependency, timeout);
		}
		if (serviceController.CanStop)
		{
			serviceController.Stop();
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
			LoggerExtensions.LogDebug(_logger, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_service_stopped, serviceController.ServiceName), Array.Empty<object>());
		}
	}

	private void ResumeServices(ServiceWithDependencies serviceWithDependencies, TimeSpan timeout)
	{
		IServiceControllerService serviceController = serviceWithDependencies.ServiceController;
		if (serviceController.Status != ServiceControllerStatus.Running)
		{
			serviceController.Start();
			serviceController.WaitForStatus(ServiceControllerStatus.Running, timeout);
			LoggerExtensions.LogDebug(_logger, string.Format(CultureInfo.CurrentCulture, IISHelper_Resources.CertificateHttps_service_started, serviceController.ServiceName), Array.Empty<object>());
		}
		foreach (ServiceWithDependencies dependency in serviceWithDependencies.Dependencies)
		{
			ResumeServices(dependency, timeout);
		}
	}

	public void Dispose()
	{
		DisposeServiceTree(_serviceWithDependencies);
	}

	private void DisposeServiceTree(ServiceWithDependencies serviceWithDependencies)
	{
		serviceWithDependencies.ServiceController.Dispose();
		foreach (ServiceWithDependencies dependency in serviceWithDependencies.Dependencies)
		{
			DisposeServiceTree(dependency);
		}
	}
}
