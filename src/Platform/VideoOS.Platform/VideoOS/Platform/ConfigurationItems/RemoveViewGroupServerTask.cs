using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RemoveViewGroupServerTask : ServerTask
{
	public bool RemoveMembers
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("RemoveMembers");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("RemoveMembers", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ItemSelectionValues => base.Wrapper.GetEnumValues("ItemSelection");

	public string ItemSelection
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ItemSelection");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ItemSelection", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public RemoveViewGroupServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RemoveViewGroup"));
	}
}
