using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddMonitorServerTask : ServerTask
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

	public Dictionary<string, string> AspectRatioValues => base.Wrapper.GetEnumValues("AspectRatio");

	public string AspectRatio
	{
		get
		{
			return base.Wrapper.GetPropertyValue("AspectRatio");
		}
		set
		{
			base.Wrapper.SetPropertyValue("AspectRatio", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> EmptyViewItemsValues => base.Wrapper.GetEnumValues("EmptyViewItems");

	public string EmptyViewItems
	{
		get
		{
			return base.Wrapper.GetPropertyValue("EmptyViewItems");
		}
		set
		{
			base.Wrapper.SetPropertyValue("EmptyViewItems", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> InsertionMethodValues => base.Wrapper.GetEnumValues("InsertionMethod");

	public string InsertionMethod
	{
		get
		{
			return base.Wrapper.GetPropertyValue("InsertionMethod");
		}
		set
		{
			base.Wrapper.SetPropertyValue("InsertionMethod", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double LocationX
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LocationX");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("LocationX", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double LocationY
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LocationY");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("LocationY", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double MonitorSize
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("MonitorSize");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("MonitorSize", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> NoLayoutValues => base.Wrapper.GetEnumValues("NoLayout");

	public string NoLayout
	{
		get
		{
			return base.Wrapper.GetPropertyValue("NoLayout");
		}
		set
		{
			base.Wrapper.SetPropertyValue("NoLayout", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddMonitorServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddMonitor"));
	}
}
