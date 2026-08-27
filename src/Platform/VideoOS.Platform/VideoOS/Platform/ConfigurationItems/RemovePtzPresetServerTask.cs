using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RemovePtzPresetServerTask : ServerTask
{
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

	public RemovePtzPresetServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("RemovePtzPreset"));
	}
}
