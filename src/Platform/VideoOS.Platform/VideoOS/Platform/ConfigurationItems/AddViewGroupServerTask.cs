using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddViewGroupServerTask : ServerTask
{
	public string Id
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Id");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Id", value);
			base.Wrapper.IsDirty = true;
		}
	}

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

	public string ViewGroupDataXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewGroupDataXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewGroupDataXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddViewGroupServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddViewGroup"));
	}
}
