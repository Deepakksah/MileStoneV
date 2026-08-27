using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CameraGroup : IConfigurationItem
{
	public CameraFolder CameraFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CameraFolder")
				{
					return new CameraFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CameraGroupFolder CameraGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CameraGroupFolder")
				{
					return new CameraGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public bool BuiltIn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BuiltIn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public CameraGroup(ServerId serverId, ConfigurationItem cameraGroup)
		: base(serverId, cameraGroup)
	{
	}

	public CameraGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CameraGroup(FQID fqid)
		: base(fqid)
	{
	}
}
