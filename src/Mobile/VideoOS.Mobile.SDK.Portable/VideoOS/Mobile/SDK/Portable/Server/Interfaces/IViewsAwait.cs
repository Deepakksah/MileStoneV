using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IViewsAwait
{
	Task<GetSubItemsResponse> GetViewsAsync(ViewParams viewParams, TimeSpan timeout);

	Task<GetAllItemsResponse> GetAllViewsAndCamerasAsync(ViewParams viewParams, TimeSpan timeout);
}
