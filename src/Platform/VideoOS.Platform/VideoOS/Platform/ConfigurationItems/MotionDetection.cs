using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MotionDetection : IConfigurationItem
{
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

	public bool ManualSensitivityEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ManualSensitivityEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManualSensitivityEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public int ManualSensitivity
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ManualSensitivity");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ManualSensitivity", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public int Threshold
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Threshold");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Threshold", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool KeyframesOnly
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("KeyframesOnly");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("KeyframesOnly", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ProcessTimeValues => base.Wrapper.GetEnumValues("ProcessTime");

	public string ProcessTime
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ProcessTime");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ProcessTime", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> DetectionMethodValues => base.Wrapper.GetEnumValues("DetectionMethod");

	public string DetectionMethod
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DetectionMethod");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DetectionMethod", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool GenerateMotionMetadata
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("GenerateMotionMetadata");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("GenerateMotionMetadata", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool UseExcludeRegions
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseExcludeRegions");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("UseExcludeRegions", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> GridSizeValues => base.Wrapper.GetEnumValues("GridSize");

	public string GridSize
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GridSize");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GridSize", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ExcludeRegions
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExcludeRegions");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExcludeRegions", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> HardwareAccelerationModeValues => base.Wrapper.GetEnumValues("HardwareAccelerationMode");

	public string HardwareAccelerationMode
	{
		get
		{
			return base.Wrapper.GetPropertyValue("HardwareAccelerationMode");
		}
		set
		{
			base.Wrapper.SetPropertyValue("HardwareAccelerationMode", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MotionDetection(ServerId serverId, ConfigurationItem motionDetection)
		: base(serverId, motionDetection)
	{
	}

	public MotionDetection(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MotionDetection(FQID fqid)
		: base(fqid)
	{
	}
}
