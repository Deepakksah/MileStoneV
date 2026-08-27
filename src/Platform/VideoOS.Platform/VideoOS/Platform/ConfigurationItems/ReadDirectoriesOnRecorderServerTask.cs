using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ReadDirectoriesOnRecorderServerTask : ServerTask
{
	public string CurrentDirectory
	{
		get
		{
			return base.Wrapper.GetPropertyValue("CurrentDirectory");
		}
		set
		{
			base.Wrapper.SetPropertyValue("CurrentDirectory", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ReadDirectoriesOnRecorderServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ReadDirectoriesOnRecorder"));
	}
}
