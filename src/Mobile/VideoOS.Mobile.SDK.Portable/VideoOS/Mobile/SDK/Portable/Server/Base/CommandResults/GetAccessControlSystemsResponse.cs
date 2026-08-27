using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlSystemsResponse : BaseCommandResponse
{
	public IEnumerable<AccessControlSystem> AccessControlSystems { get; private set; }

	public GetAccessControlSystemsResponse(Command command)
		: base(command)
	{
		AccessControlSystems = SerializableAccessControl.GetAccessControlSystems(command.Items);
	}

	public GetAccessControlSystemsResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
