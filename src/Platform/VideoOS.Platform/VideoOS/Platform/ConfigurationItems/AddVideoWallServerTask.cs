using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddVideoWallServerTask : ServerTask
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

	public AddVideoWallServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddVideoWall"));
	}
}
