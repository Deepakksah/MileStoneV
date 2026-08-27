using System.Collections.ObjectModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("RecordingServerInformation")]
public sealed class RecordingServerInformation
{
	public ReadOnlyCollection<RecordingServerDiskUsage> DiskUsage { get; private set; }

	public RecordingServerInformation(VmoPropertyCollection objectProperties)
	{
		DiskUsage = new ReadOnlyCollection<RecordingServerDiskUsage>(objectProperties.GetPropertyValue<RecordingServerDiskUsages>("DiskUsage"));
	}
}
