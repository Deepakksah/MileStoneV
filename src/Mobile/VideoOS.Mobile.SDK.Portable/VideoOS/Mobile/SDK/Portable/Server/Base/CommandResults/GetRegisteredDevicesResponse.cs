using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetRegisteredDevicesResponse : BaseCommandResponse
{
	public List<RegisteredDevice> RegisteredDevices { get; private set; }

	public GetRegisteredDevicesResponse(Command command)
		: base(command)
	{
		RegisteredDevices = command.RegisteredDevices?.RegisteredDevicesList;
	}

	public GetRegisteredDevicesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
