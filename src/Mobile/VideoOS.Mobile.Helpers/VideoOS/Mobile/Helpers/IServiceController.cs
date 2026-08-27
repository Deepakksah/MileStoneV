using System;
using System.ServiceProcess;

namespace VideoOS.Mobile.Helpers;

public interface IServiceController : IDisposable
{
	ServiceControllerStatus Status { get; }

	string ServiceName { get; set; }

	string MachineName { get; set; }

	string DisplayName { get; set; }

	bool CanStop { get; }

	bool CanShutdown { get; }

	bool CanPauseAndContinue { get; }

	ServiceType ServiceType { get; }

	ServiceStartMode StartType { get; }

	void Close();

	void Continue();

	void Pause();

	void Start();

	void Start(string[] args);

	void Stop();

	void WaitForStatus(ServiceControllerStatus desiredStatus);

	void WaitForStatus(ServiceControllerStatus desiredStatus, TimeSpan timeout);
}
