using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MoveRecordingStorageServerTask : ServerTask
{
	public string DiskPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DiskPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DiskPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MoveRecordingStorageServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MoveRecordingStorage"));
	}
}
