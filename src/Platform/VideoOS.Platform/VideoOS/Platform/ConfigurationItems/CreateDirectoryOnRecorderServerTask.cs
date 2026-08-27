using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CreateDirectoryOnRecorderServerTask : ServerTask
{
	public string NewDirectoryFullName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("NewDirectoryFullName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("NewDirectoryFullName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public CreateDirectoryOnRecorderServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("CreateDirectoryOnRecorder"));
	}
}
