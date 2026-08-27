using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IBookmarkAsync : IBookmarkCreateAsync
{
	IAsyncResult UpdateBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult DeleteBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
