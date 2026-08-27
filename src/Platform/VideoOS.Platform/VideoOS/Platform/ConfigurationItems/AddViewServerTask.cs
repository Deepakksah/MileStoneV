using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddViewServerTask : ClassCollectionServerTask
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

	public string Shortcut
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Shortcut");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Shortcut", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ViewLayoutType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewLayoutType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewLayoutType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutCustomId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutCustomId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutCustomId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutIcon
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutIcon");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutIcon", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string LayoutViewItems
	{
		get
		{
			return base.Wrapper.GetPropertyValue("LayoutViewItems");
		}
		set
		{
			base.Wrapper.SetPropertyValue("LayoutViewItems", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ClientId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ClientId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ClientId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddViewServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddView"));
	}
}
