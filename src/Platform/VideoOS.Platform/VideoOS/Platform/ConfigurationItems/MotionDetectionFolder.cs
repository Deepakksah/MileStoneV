using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MotionDetectionFolder : IConfigurationItem
{
	public ICollection<MotionDetection> MotionDetections
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<MotionDetection> list = new List<MotionDetection>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "MotionDetection")
				{
					list.Add(new MotionDetection(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public MotionDetectionFolder(ServerId serverId, ConfigurationItem motionDetectionFolder)
		: base(serverId, motionDetectionFolder)
	{
	}

	public MotionDetectionFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MotionDetectionFolder(FQID fqid)
		: base(fqid)
	{
	}
}
