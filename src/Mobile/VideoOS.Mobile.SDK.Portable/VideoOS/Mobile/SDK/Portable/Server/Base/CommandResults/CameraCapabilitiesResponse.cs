using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class CameraCapabilitiesResponse : BaseCommandResponse
{
	public CameraCapabilities Capabilities { get; private set; }

	public CameraCapabilitiesResponse(Command command)
		: base(command)
	{
		Capabilities = new CameraCapabilities();
		Dictionary<string, string> deviceParams = command.ConvertToDictionary(Command.ParamsType.Output);
		Capabilities.InitFromCommand(deviceParams);
	}

	public CameraCapabilitiesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
