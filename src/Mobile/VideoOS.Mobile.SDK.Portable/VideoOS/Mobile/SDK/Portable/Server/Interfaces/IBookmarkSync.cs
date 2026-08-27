using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IBookmarkSync : IBookmarkCreateSync
{
	BaseCommandResponse UpdateBookmark(BookmarkParams bookmarkParams, TimeSpan timeout);

	BaseCommandResponse DeleteBookmark(BookmarkParams bookmarkParams, TimeSpan timeout);
}
