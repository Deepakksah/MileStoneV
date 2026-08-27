using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class BasicOwnerInformationFolder : IConfigurationItem
{
	public ICollection<BasicOwnerInformation> BasicOwnerInformations
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<BasicOwnerInformation> list = new List<BasicOwnerInformation>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "BasicOwnerInformation")
				{
					list.Add(new BasicOwnerInformation(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public BasicOwnerInformationFolder(ServerId serverId, ConfigurationItem basicOwnerInformationFolder)
		: base(serverId, basicOwnerInformationFolder)
	{
	}

	public BasicOwnerInformationFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public BasicOwnerInformationFolder(FQID fqid)
		: base(fqid)
	{
	}

	public BasicOwnerInformationFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/BasicOwnerInformationFolder")
	{
	}
}
