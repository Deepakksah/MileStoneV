using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MIPItem : IConfigurationItem, IMIPItem
{
	private ConfigurationApiProperties _properties;

	public MIPItemFolder MIPItemFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MIPItemFolder")
				{
					return new MIPItemFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string GisPoint
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GisPoint");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GisPoint", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LastModified
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LastModified");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LastModified", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ConfigurationApiProperties Properties => _properties;

	public MIPItem(ServerId serverId, ConfigurationItem mipItem)
		: base(serverId, mipItem)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public MIPItem(ServerId serverId, string path)
		: base(serverId, path)
	{
		_properties = new ConfigurationApiProperties(base.Wrapper);
	}

	public MIPItem(ServerId serverId, Guid objectId)
		: this(serverId, string.Format(CultureInfo.InvariantCulture, "MIPItem[{0}]", objectId))
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
			EnvironmentManager.Instance.Log(error: false, "MIPItem", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "MIPItem", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}
}
