using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetBannersResponse : BaseCommandResponse
{
	public IEnumerable<SerializableItem> BannerList { get; private set; }

	public GetBannersResponse(Command command)
		: base(command)
	{
		BannerList = command.Items;
	}

	public GetBannersResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
