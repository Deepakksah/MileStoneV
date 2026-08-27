using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServerFailoverFolder : IConfigurationItem
{
	public ICollection<RecordingServerFailover> RecordingServerFailovers
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<RecordingServerFailover> list = new List<RecordingServerFailover>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "RecordingServerFailover")
				{
					list.Add(new RecordingServerFailover(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public RecordingServerFailoverFolder(ServerId serverId, ConfigurationItem recordingServerFailoverFolder)
		: base(serverId, recordingServerFailoverFolder)
	{
	}

	public RecordingServerFailoverFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServerFailoverFolder(FQID fqid)
		: base(fqid)
	{
	}
}
