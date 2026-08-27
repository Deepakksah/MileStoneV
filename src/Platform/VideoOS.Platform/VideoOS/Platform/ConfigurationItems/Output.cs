using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Output : IConfigurationItem
{
	public CustomPropertiesFolder CustomPropertiesFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CustomPropertiesFolder")
				{
					return new CustomPropertiesFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public DeviceDriverSettingsFolder DeviceDriverSettingsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "DeviceDriverSettingsFolder")
				{
					return new DeviceDriverSettingsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwareDeviceEventFolder HardwareDeviceEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareDeviceEventFolder")
				{
					return new HardwareDeviceEventFolder(base.ServerId, configurationItem);
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

	public int Channel
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Channel");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
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

	public string ShortName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ShortName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ShortName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Icon
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Icon");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Icon", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageDirection
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageDirection");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageDirection", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageDepth
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageDepth");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageDepth", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double CoverageFieldOfView
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CoverageFieldOfView");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CoverageFieldOfView", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Output(ServerId serverId, ConfigurationItem output)
		: base(serverId, output)
	{
	}

	public Output(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Output(FQID fqid)
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
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
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
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'ChangeSecurityPermissions':" + ex.Message);
		}
		return null;
	}

	public ServerTask Trigger()
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("Trigger"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'Trigger':" + ex.Message);
		}
		return null;
	}

	public ServerTask Activate()
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("Activate"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'Activate':" + ex.Message);
		}
		return null;
	}

	public ServerTask Deactivate()
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("Deactivate"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'Deactivate':" + ex.Message);
		}
		return null;
	}

	public ServerTask SendDriverCommand(string command, string parameter, int timeOut)
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("SendDriverCommand");
			configurationApiItemWrapper.SetProperty("command", command);
			configurationApiItemWrapper.SetProperty("parameter", parameter);
			configurationApiItemWrapper.SetProperty("timeOut", timeOut.ToString(CultureInfo.InvariantCulture));
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("SendDriverCommand"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Output", "Unable to call method 'SendDriverCommand':" + ex.Message);
		}
		return null;
	}
}
