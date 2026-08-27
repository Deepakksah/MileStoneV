using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetPermittedUsersResponse : BaseCommandResponse
{
	public List<string> PermittedUsersList { get; private set; }

	public GetPermittedUsersResponse(Command command)
		: base(command)
	{
		PermittedUsersList = command.Items?.Select((SerializableItem item) => item.Properties["Username"]).ToList();
	}

	public GetPermittedUsersResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
