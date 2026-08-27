using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IItemsAsync
{
	IAsyncResult GetItems(ItemParams itemParams, OnResponse<GetAllItemsResponse> onSuccess = null, OnResponse<GetAllItemsResponse> onFail = null);
}
