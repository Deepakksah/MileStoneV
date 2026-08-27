using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PtzPreset : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool DevicePreset
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DevicePreset");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public string DevicePresetInternalId => base.Wrapper.GetPropertyValue("DevicePresetInternalId");

	public bool DefaultPreset
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DefaultPreset");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public bool Locked
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Locked");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Locked", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public double Pan
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Pan");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Pan", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double Tilt
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Tilt");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Tilt", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double Zoom
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Zoom");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Zoom", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public PtzPreset(ServerId serverId, ConfigurationItem ptzPreset)
		: base(serverId, ptzPreset)
	{
	}

	public PtzPreset(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PtzPreset(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask Goto()
	{
		if (!Configuration.Instance.ConfigurationApiManager.UseRestApiWhenAvailable)
		{
			throw new MIPException("This method is only supported when running with the REST API");
		}
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("Goto"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "PtzPreset", "Unable to call method 'Goto':" + ex.Message);
		}
		return null;
	}
}
