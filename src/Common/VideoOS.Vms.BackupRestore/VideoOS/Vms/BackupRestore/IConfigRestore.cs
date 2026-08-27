using System.Collections.Generic;

namespace VideoOS.Vms.BackupRestore;

public interface IConfigRestore
{
	string ComponentName { get; }

	RestoreValidationResult ValidateForRestore(string configData, IList<RegisteredService> registeredServices, ReportComponentStatus reportStatus, AbortRequested abortRequested);

	RestoreComponentResult Restore(string path, ReportComponentStatus reportStatus, AbortRequested abortRequested);

	bool StopServices(ReportComponentStatus reportStatus);

	bool StartServices(ReportComponentStatus reportStatus);

	string[] GetServiceAccounts(IList<RegisteredService> registeredServices);
}
