using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddPatrollingProfileServerTask : ServerTask
{
	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Description
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Description");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Description", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public bool CustomizeTransitions
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CustomizeTransitions");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CustomizeTransitions", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public double InitSpeed
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("InitSpeed");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("InitSpeed", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double InitTransitionTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("InitTransitionTime");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("InitTransitionTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EndPresetIdValues => base.Wrapper.GetEnumValues("EndPresetId");

	public string EndPresetId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EndPresetId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndPresetId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double EndSpeed
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EndSpeed");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndSpeed", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double EndTransitionTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("EndTransitionTime");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("EndTransitionTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public AddPatrollingProfileServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddPatrollingProfile"));
	}
}
