using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlDoorTypesResponse : BaseCommandResponse
{
	public IEnumerable<AccessControlDoorType> AccessControlDoorTypes { get; private set; }

	public GetAccessControlDoorTypesResponse(Command command)
		: base(command)
	{
		AccessControlDoorTypes = SerializableAccessControl.GetAccessControlDoorTypes(command.Items);
	}

	public GetAccessControlDoorTypesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
