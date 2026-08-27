using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AutoDetectConfiguration : IConfigurationItem
{
	public DetectedDeviceCredentialsFolder DetectedDeviceCredentialsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "DetectedDeviceCredentialsFolder")
				{
					return new DetectedDeviceCredentialsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool Enabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Enabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Enabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public AutoDetectConfiguration(ServerId serverId, ConfigurationItem autoDetectConfiguration)
		: base(serverId, autoDetectConfiguration)
	{
	}

	public AutoDetectConfiguration(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public AutoDetectConfiguration(FQID fqid)
		: base(fqid)
	{
	}
}
