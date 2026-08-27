using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class FisheyeLens : IConfigurationItem
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

	public Dictionary<string, string> RegisteredPanomorphLensValues => base.Wrapper.GetEnumValues("RegisteredPanomorphLens");

	public string RegisteredPanomorphLens
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RegisteredPanomorphLens");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RegisteredPanomorphLens", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> OrientationValues => base.Wrapper.GetEnumValues("Orientation");

	public string Orientation
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Orientation");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Orientation", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> RegisteredPanomorphLensNumberValues => base.Wrapper.GetEnumValues("RegisteredPanomorphLensNumber");

	public string RegisteredPanomorphLensNumber
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RegisteredPanomorphLensNumber");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RegisteredPanomorphLensNumber", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double FisheyeFieldOfView
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FisheyeFieldOfView");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FisheyeFieldOfView", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public FisheyeLens(ServerId serverId, ConfigurationItem fisheyeLens)
		: base(serverId, fisheyeLens)
	{
	}

	public FisheyeLens(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public FisheyeLens(FQID fqid)
		: base(fqid)
	{
	}
}
