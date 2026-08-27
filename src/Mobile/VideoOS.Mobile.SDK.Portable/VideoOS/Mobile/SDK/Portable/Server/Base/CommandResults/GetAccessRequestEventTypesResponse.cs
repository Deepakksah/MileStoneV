using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessRequestEventTypesResponse : BaseCommandResponse
{
	public IEnumerable<AccessControlEventType> AccessControlEventTypes { get; private set; }

	public GetAccessRequestEventTypesResponse(Command command)
		: base(command)
	{
		AccessControlEventTypes = SerializableAccessControl.GetAccessControlEventTypes(command.Items);
	}

	public GetAccessRequestEventTypesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
