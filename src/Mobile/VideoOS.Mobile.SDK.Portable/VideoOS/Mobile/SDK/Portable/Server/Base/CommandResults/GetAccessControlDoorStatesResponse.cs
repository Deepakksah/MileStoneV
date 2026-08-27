using System.Linq;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlDoorStatesResponse : BaseCommandResponse
{
	public AccessControlDoorStates AccessControlDoorStates { get; private set; }

	public GetAccessControlDoorStatesResponse(Command command)
		: base(command)
	{
		AccessControlDoorStates = SerializableAccessControl.GetAccessControlDoorStates(command.Items?.First());
	}

	public GetAccessControlDoorStatesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
