using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IViewsAsync
{
	IAsyncResult GetViews(ViewParams viewParams, OnResponse<GetSubItemsResponse> onSuccess = null, OnResponse<GetSubItemsResponse> onFail = null);

	IAsyncResult GetAllViewsAndCameras(ViewParams viewParams, OnResponse<GetAllItemsResponse> onSuccess = null, OnResponse<GetAllItemsResponse> onFail = null);
}
