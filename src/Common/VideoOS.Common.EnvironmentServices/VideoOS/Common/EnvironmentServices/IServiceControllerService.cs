using System;
using System.ServiceProcess;

namespace VideoOS.Common.EnvironmentServices;

public interface IServiceControllerService : IDisposable
{
	string ServiceName { get; set; }

	ServiceControllerStatus Status { get; }

	bool CanStop { get; }

	IServiceControllerService[] DependentServices { get; }

	void Start();

	void Stop();

	void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout);
}
