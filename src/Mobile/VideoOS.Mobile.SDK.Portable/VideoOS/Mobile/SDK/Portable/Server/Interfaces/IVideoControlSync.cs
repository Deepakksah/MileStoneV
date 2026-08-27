using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

namespace VideoOS.Mobile.SDK.Portable.Server.Interfaces;

public interface IVideoControlSync : IStreamControlSync<VideoParams>
{
	BaseCommandResponse ControlCarousel(CarouselParams carouselParams, TimeSpan timeout);
}
