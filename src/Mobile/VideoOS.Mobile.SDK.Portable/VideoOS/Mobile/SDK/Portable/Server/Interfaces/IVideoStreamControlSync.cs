using System;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoStreamControlSync
{
	BaseCommandResponse Rescale(int width, int height, TimeSpan timeout);

	BaseCommandResponse Crop(int left, int top, int right, int bottom, TimeSpan timeout);
}
