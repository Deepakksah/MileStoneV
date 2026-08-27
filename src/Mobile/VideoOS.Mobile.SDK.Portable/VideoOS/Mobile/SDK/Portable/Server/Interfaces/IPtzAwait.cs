using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPtzAwait
{
	Task<BaseCommandResponse> ControlPtzAsync(PtzParams ptzParams, TimeSpan timeout);

	Task<GetPtzPresetsResponse> GetPtzPresetsAsync(Guid cameraId, TimeSpan timeout);
}
