using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class AddressBookServiceResponse : BaseCommandResponse
{
	public ServerConfigurationParams ServerConfigurationParams { get; private set; }

	public AddressBookServiceResponse(Command command)
		: base(command)
	{
		ServerConfigurationParams = new ServerConfigurationParams(command, 2047u, Command.ParamsType.Output);
	}

	public AddressBookServiceResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
