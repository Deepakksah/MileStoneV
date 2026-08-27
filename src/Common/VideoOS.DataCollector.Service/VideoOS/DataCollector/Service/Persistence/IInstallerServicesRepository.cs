using System.Collections.Concurrent;

namespace VideoOS.DataCollector.Service.Persistence;

public interface IInstallerServicesRepository
{
	ConcurrentDictionary<string, string> Services { get; set; }
}
