using System;
using System.Threading.Tasks;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service.Licenses.Providers;
using VideoOS.IM.Server.Service.OEM;

namespace VideoOS.IM.Server.Service.Licenses.Validators;

public interface ILicenseValidationStatusService
{
	bool IsLicenseValid { get; }

	bool IsLicenseValidationRunning { get; }

	void InitVerifyLicenseValidationStatus([NotNull] ILicenseProvider licenseProvider, [NotNull] ILoggerProvider loggerProvider, [NotNull] IOemResourceService oemResourceService);

	Task StartVerifyLicenseValidationStatusAsync(TimeSpan? interval = null);

	void StopVerifyLicenseValidationStatus();
}
