using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class DetectedDeviceCredentials : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string UserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("UserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("UserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Password
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Password");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Password", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DetectedDeviceCredentials(ServerId serverId, ConfigurationItem detectedDeviceCredentials)
		: base(serverId, detectedDeviceCredentials)
	{
	}

	public DetectedDeviceCredentials(ServerId serverId, string path)
		: base(serverId, path)
	{
	}
}
