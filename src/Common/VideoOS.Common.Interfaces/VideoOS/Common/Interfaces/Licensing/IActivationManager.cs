using System;

namespace VideoOS.Common.Interfaces.Licensing;

public interface IActivationManager : IDisposable
{
	bool IsActivationQueued { get; }

	Exception LastAutomaticActivationError { get; }

	DateTime NextActivation { get; }

	bool SystemActivation { get; set; }

	event EventHandler AutomaticActivationFailedEventHandler;

	void ActivateLicense(string userName, string password);

	bool QueryAutomaticLicenseActivationEnabled();

	void QueueAutomaticActivation();
}
