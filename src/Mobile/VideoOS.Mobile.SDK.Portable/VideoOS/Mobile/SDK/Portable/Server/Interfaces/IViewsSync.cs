using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IViewsSync
{
	GetSubItemsResponse GetViews(ViewParams viewParams, TimeSpan timeout);

	GetAllItemsResponse GetAllViewsAndCameras(ViewParams viewParams, TimeSpan timeout);
}
