using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PrivacyProtectionFolder : IConfigurationItem
{
	public ICollection<PrivacyProtection> PrivacyProtections
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<PrivacyProtection> list = new List<PrivacyProtection>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "PrivacyProtection")
				{
					list.Add(new PrivacyProtection(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public PrivacyProtectionFolder(ServerId serverId, ConfigurationItem privacyProtectionFolder)
		: base(serverId, privacyProtectionFolder)
	{
	}

	public PrivacyProtectionFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PrivacyProtectionFolder(FQID fqid)
		: base(fqid)
	{
	}
}
