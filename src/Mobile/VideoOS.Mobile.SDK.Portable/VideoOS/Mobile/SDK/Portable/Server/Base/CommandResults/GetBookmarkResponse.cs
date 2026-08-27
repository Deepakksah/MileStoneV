using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class GetBookmarkResponse : BaseCommandResponse
{
	public IEnumerable<IBookmarkEntry> Bookmarks { get; }

	public GetBookmarkResponse(Command command)
		: base(command)
	{
		Bookmarks = SerializableBookmark.GetBookmarks(command.Items);
	}

	public GetBookmarkResponse(BaseCommandResponse other)
		: this(other.Command)
	{
	}
}
