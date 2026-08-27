using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using VideoOS.DataCollector.Oem;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterServiceApplications;

public class RegisterInstalledServicesService : IRegisterInstalledServicesService
{
	private readonly IFailoverServerResources _failoverServerResources;

	private readonly IServiceControlManager _serviceControlManager;

	private readonly INativeMethods _nativeMethods;

	private readonly IWebHostEnvironment _hostEnvironment;

	public RegisterInstalledServicesService(IFailoverServerResources failoverServerResources, IServiceControlManager serviceControlManager, INativeMethods nativeMethods, IWebHostEnvironment hostEnvironment)
	{
		_failoverServerResources = failoverServerResources;
		_serviceControlManager = serviceControlManager;
		_nativeMethods = nativeMethods;
		_hostEnvironment = hostEnvironment;
	}

	public ServiceApplication[] EnumServiceApplications(ref ServiceApplication[] applicationList)
	{
		if (_hostEnvironment.IsDevelopment())
		{
			ServiceApplication[] array = applicationList;
			foreach (ServiceApplication serviceApplication in array)
			{
				serviceApplication.IsInstalled = true;
			}
		}
		else
		{
			ServiceApplication[] array2 = applicationList;
			foreach (ServiceApplication serviceApplication2 in array2)
			{
				if (serviceApplication2.ApplicationName.Equals("FailoverServer"))
				{
					serviceApplication2.IsInstalled = _serviceControlManager.IsServiceInstalled(_failoverServerResources.FailoverServiceName);
				}
				else if (IsProductInstalled(serviceApplication2.UpgradeCode))
				{
					serviceApplication2.IsInstalled = true;
				}
			}
		}
		return applicationList;
	}

	private bool IsProductInstalled(string productId)
	{
		string value = _nativeMethods.EnumRelatedProducts(productId);
		return !string.IsNullOrEmpty(value);
	}
}
