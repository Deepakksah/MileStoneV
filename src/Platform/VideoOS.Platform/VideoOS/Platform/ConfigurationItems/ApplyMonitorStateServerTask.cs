using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ApplyMonitorStateServerTask : ServerTask
{
	public string MonitorState
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MonitorState");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MonitorState", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ApplyMonitorStateServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ApplyMonitorState"));
	}
}
