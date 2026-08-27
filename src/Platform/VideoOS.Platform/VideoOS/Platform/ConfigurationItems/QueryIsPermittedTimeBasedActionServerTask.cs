using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class QueryIsPermittedTimeBasedActionServerTask : ServerTask
{
	public string RelatedObjectPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("RelatedObjectPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("RelatedObjectPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string ActionId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ActionId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ActionId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public QueryIsPermittedTimeBasedActionServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("QueryIsPermittedTimeBasedAction"));
	}
}
