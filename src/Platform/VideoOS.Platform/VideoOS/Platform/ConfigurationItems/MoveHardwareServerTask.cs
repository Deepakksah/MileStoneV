using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MoveHardwareServerTask : ServerTask
{
	public Dictionary<string, string> DestinationRecordingServerValues => base.Wrapper.GetEnumValues("DestinationRecordingServer");

	public string DestinationRecordingServer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DestinationRecordingServer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DestinationRecordingServer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MoveHardwareServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MoveHardware"));
	}
}
