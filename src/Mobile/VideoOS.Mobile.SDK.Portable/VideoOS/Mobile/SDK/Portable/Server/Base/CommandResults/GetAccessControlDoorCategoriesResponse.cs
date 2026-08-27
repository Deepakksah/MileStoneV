using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlDoorCategoriesResponse : BaseCommandResponse
{
	public IEnumerable<AccessControlDoorCategory> AccessControlDoorCategories { get; private set; }

	public GetAccessControlDoorCategoriesResponse(Command command)
		: base(command)
	{
		AccessControlDoorCategories = SerializableAccessControl.GetAccessControlDoorCategories(command.Items);
	}

	public GetAccessControlDoorCategoriesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
