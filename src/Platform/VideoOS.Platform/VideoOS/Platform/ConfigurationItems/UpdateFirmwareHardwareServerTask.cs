using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class UpdateFirmwareHardwareServerTask : ServerTask
{
	public string StorageId
	{
		get
		{
			return base.Wrapper.GetPropertyValue("StorageId");
		}
		set
		{
			base.Wrapper.SetPropertyValue("StorageId", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public UpdateFirmwareHardwareServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("UpdateFirmwareHardware"));
	}
}
