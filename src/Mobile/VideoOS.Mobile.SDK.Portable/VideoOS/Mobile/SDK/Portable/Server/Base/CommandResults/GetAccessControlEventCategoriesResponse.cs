using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetAccessControlEventCategoriesResponse : BaseCommandResponse
{
	public IEnumerable<AccessControlEventCategory> AccessControlEventCategories { get; private set; }

	public GetAccessControlEventCategoriesResponse(Command command)
		: base(command)
	{
		AccessControlEventCategories = SerializableAccessControl.GetAccessControlEventCategories(command.Items);
	}

	public GetAccessControlEventCategoriesResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
