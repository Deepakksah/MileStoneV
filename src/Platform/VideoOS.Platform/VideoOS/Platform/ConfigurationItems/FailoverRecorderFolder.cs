using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FailoverRecorderFolder : IConfigurationItem
{
	public ICollection<FailoverRecorder> FailoverRecorders
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<FailoverRecorder> list = new List<FailoverRecorder>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "FailoverRecorder")
				{
					list.Add(new FailoverRecorder(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public FailoverRecorderFolder(ServerId serverId, ConfigurationItem failoverRecorderFolder)
		: base(serverId, failoverRecorderFolder)
	{
	}

	public FailoverRecorderFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FailoverRecorderFolder(FQID fqid)
		: base(fqid)
	{
	}
}
