using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IPtzSync
{
	BaseCommandResponse ControlPtz(PtzParams ptzParams, TimeSpan timeout);

	GetPtzPresetsResponse GetPtzPresets(Guid cameraId, TimeSpan timeout);
}
