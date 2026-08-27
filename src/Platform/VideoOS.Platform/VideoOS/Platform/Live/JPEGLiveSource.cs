using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class JPEGLiveSource : VideoLiveSource
{
	public JPEGLiveSource(Item item)
		: base(item)
	{
		base.KeyFramesOnly = false;
		new TrackPageViewData(typeof(JPEGLiveSource)).Send();
	}

	protected override object ConnectLiveSource(string media)
	{
		InternalMediaService.LiveVideoConnectParams parameters = new InternalMediaService.LiveVideoConnectParams
		{
			DeviceFqid = Item.FQID,
			Width = base.Width,
			Height = base.Height,
			KeepAspectRatio = base.KeepAspectRatio,
			FillSpace = base.FillWithBlack,
			KeyFramesOnly = base.KeyFramesOnly,
			StreamId = base.StreamId,
			Fps = base.FPS,
			AllowUpscaling = base.AllowUpscaling,
			EnableMulticast = base.MulticastEnabled,
			ConnectTimeout = base.ConnectTimeout,
			SendInitialImage = base.SendInitialImage
		};
		return InternalMediaService.Instance.LiveVideoConnect(parameters);
	}
}
