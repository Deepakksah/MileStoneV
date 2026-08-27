using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddTimeProfileServerTask : ServerTask
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

	public Dictionary<string, string> TimeProfileTypeValues => base.Wrapper.GetEnumValues("TimeProfileType");

	public string TimeProfileType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TimeProfileType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TimeProfileType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddTimeProfileServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddTimeProfile"));
	}
}
