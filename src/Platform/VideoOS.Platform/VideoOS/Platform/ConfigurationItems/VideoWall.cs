using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class VideoWall : IConfigurationItem
{
	public MonitorFolder MonitorFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MonitorFolder")
				{
					return new MonitorFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public VideoWallPresetFolder VideoWallPresetFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "VideoWallPresetFolder")
				{
					return new VideoWallPresetFolder(base.ServerId, configurationItem);
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

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public double Height
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Height");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
	}

	public bool StatusText
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("StatusText");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("StatusText", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> TitleBarValues => base.Wrapper.GetEnumValues("TitleBar");

	public string TitleBar
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TitleBar");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TitleBar", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double Width
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Width");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
	}

	public VideoWall(ServerId serverId, ConfigurationItem videoWall)
		: base(serverId, videoWall)
	{
	}

	public VideoWall(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public VideoWall(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ChangeSecurityPermissions(string userPath)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangeSecurityPermissions");
			configurationApiItemWrapper.SetProperty("UserPath", userPath);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWall", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ChangeSecurityPermissionsServerTask ChangeSecurityPermissions()
	{
		try
		{
			return new ChangeSecurityPermissionsServerTask(base.Wrapper.ExecuteMethod("ChangeSecurityPermissions"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "VideoWall", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
