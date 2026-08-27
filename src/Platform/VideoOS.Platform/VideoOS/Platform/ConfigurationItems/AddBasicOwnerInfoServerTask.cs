using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddBasicOwnerInfoServerTask : ServerTask
{
	public Dictionary<string, string> TagTypeValues => base.Wrapper.GetEnumValues("TagType");

	public string TagType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TagType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TagType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string TagValue
	{
		get
		{
			return base.Wrapper.GetPropertyValue("TagValue");
		}
		set
		{
			base.Wrapper.SetPropertyValue("TagValue", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddBasicOwnerInfoServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddBasicOwnerInfo"));
	}
}
