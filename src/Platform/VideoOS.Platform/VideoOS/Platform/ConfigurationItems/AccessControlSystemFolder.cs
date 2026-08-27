using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AccessControlSystemFolder : IConfigurationItem
{
	public ICollection<AccessControlSystem> AccessControlSystems
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			List<AccessControlSystem> list = new List<AccessControlSystem>();
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem configurationItem in array2)
			{
				if (configurationItem.ItemType == "AccessControlSystem")
				{
					list.Add(new AccessControlSystem(base.ServerId, configurationItem));
				}
			}
			return list;
		}
	}

	public string RuleGroup => base.Wrapper.GetPropertyValue("RuleGroup");

	public AccessControlSystemFolder(ServerId serverId, ConfigurationItem accessControlSystemFolder)
		: base(serverId, accessControlSystemFolder)
	{
	}

	public AccessControlSystemFolder(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AccessControlSystemFolder(FQID fqid)
		: base(fqid)
	{
	}

	public AccessControlSystemFolder()
		: base(EnvironmentManager.Instance.MasterSite.ServerId, "/AccessControlSystemFolder")
	{
	}
}
