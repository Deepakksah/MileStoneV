using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServerMulticastFolder : IConfigurationItem
{
	public ICollection<RecordingServerMulticast> RecordingServerMulticasts
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<RecordingServerMulticast> list = new List<RecordingServerMulticast>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "RecordingServerMulticast")
				{
					list.Add(new RecordingServerMulticast(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public RecordingServerMulticastFolder(ServerId serverId, ConfigurationItem recordingServerMulticastFolder)
		: base(serverId, recordingServerMulticastFolder)
	{
	}

	public RecordingServerMulticastFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServerMulticastFolder(FQID fqid)
		: base(fqid)
	{
	}
}
