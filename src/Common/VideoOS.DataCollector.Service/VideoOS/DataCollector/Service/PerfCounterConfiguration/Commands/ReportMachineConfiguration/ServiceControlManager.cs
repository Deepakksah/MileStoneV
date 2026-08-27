using System;
using System.Linq;
using System.Net.Http;
using System.ServiceProcess;
using System.Threading;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

public class ServiceControlManager : IServiceControlManager
{
	public bool IsServiceInstalled(string serviceName)
	{
		ServiceController[] services = ServiceController.GetServices();
		int? num = (from y in services
			where y.ServiceName.Equals(serviceName, StringComparison.OrdinalIgnoreCase)
			select (y))?.Count();
		ServiceController[] array = services;
		foreach (ServiceController serviceController in array)
		{
			serviceController.Dispose();
		}
		return num > 0;
	}

	public bool IsRunning(string serviceName)
	{
		if (IsServiceIis(serviceName))
		{
			using (HttpClient httpClient = new HttpClient())
			{
				try
				{
					Uri requestUri = new Uri(serviceName);
					CancellationToken token = new CancellationTokenSource(TimeSpan.FromSeconds(10.0)).Token;
					HttpResponseMessage result = httpClient.GetAsync(requestUri, token).GetAwaiter().GetResult();
					return result.IsSuccessStatusCode;
				}
				catch (Exception)
				{
					return false;
				}
			}
		}
		using ServiceController serviceController = new ServiceController(serviceName);
		try
		{
			ServiceControllerStatus status = serviceController.Status;
			if (status == ServiceControllerStatus.Running)
			{
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private static bool IsServiceIis(string serviceName)
	{
		if (!serviceName.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
		{
			return serviceName.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}
}
