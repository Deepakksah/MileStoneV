using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RemoveBasicOwnerInfoServerTask : ServerTask
{
	[Obsolete("Deprecated. Please use ItemSelectionValues instead")]
	public Dictionary<string, string> SelectToRemoveValues => base.Wrapper.GetEnumValues("SelectToRemove");

	[Obsolete("Deprecated. Please use ItemSelection instead")]
	public string SelectToRemove
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SelectToRemove");
		}
		set
		{
			base.Wrapper.SetPropertyValue("SelectToRemove", value);
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

	public RemoveBasicOwnerInfoServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RemoveBasicOwnerInfo"));
	}
}
