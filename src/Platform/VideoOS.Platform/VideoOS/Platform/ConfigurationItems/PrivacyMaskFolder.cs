using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PrivacyMaskFolder : IConfigurationItem
{
	public ICollection<PrivacyMask> PrivacyMasks
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PrivacyMask> list = new List<PrivacyMask>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PrivacyMask")
				{
					list.Add(new PrivacyMask(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public PrivacyMaskFolder(ServerId serverId, ConfigurationItem privacyMaskFolder)
		: base(serverId, privacyMaskFolder)
	{
	}

	public PrivacyMaskFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PrivacyMaskFolder(FQID fqid)
		: base(fqid)
	{
	}
}
