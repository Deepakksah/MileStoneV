using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface ISystemMonitorEventQueueService
{
	BulkProcessor<PluginEventInfo> EventQueue { get; }
}
