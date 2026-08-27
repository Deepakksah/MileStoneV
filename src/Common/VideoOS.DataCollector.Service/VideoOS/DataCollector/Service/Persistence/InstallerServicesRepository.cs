using System.Collections.Concurrent;

namespace VideoOS.DataCollector.Service.Persistence;

public class InstallerServicesRepository : IInstallerServicesRepository
{
	public ConcurrentDictionary<string, string> Services { get; set; } = new ConcurrentDictionary<string, string>();
}
