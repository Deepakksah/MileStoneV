using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetConfigurationResponse : BaseCommandResponse
{
	public ConfigurationItem ConfigurationItem { get; private set; }

	public GetConfigurationResponse(Command command)
		: base(command)
	{
		ConfigurationItem = command.Configuration;
	}

	public GetConfigurationResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
