using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPtzAsync
{
	IAsyncResult ControlPtz(PtzParams ptzParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null);

	IAsyncResult GetPtzPresets(Guid cameraId, OnResponse<GetPtzPresetsResponse> onSuccess = null, OnResponse<GetPtzPresetsResponse> onFail = null);
}
