using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddGisMapLocationServerTask : ServerTask
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

	public double Scale
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Scale");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Scale", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string Color
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Color");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Color", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public double PositionX
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PositionX");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PositionX", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public double PositionY
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PositionY");
			double result = 0.0;
			if (double.TryParse(propertyValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				return result;
			}
			return 0.0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PositionY", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public AddGisMapLocationServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddGisMapLocation"));
	}
}
