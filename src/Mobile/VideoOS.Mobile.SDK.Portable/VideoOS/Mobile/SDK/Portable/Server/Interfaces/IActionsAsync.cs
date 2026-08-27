using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IActionsAsync
{
	IAsyncResult GetActions(OutputsEventsParams actionsParams, OnResponse<GetActionsResponse> onSuccess = null, OnResponse<GetActionsResponse> onFail = null);

	IAsyncResult ActivateAction(ActivateParams activateParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);
}
