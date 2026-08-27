using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class BitmapLiveSource : VideoLiveSource
{
	private readonly BitmapFormat _bitmapFormat;

	public BitmapLiveSource(Item item, BitmapFormat bitmapFormat)
		: base(item)
	{
		_bitmapFormat = bitmapFormat;
		new TrackPageViewData(typeof(BitmapLiveSource)).Send();
	}

	protected override object ConnectLiveSource(string media)
	{
		InternalMediaService.LiveVideoConnectBitmapParams connectParams = new InternalMediaService.LiveVideoConnectBitmapParams
		{
			DeviceFqid = Item.FQID,
			Width = base.Width,
			Height = base.Height,
			KeepAspectRatio = base.KeepAspectRatio,
			FillSpace = base.FillWithBlack,
			KeyFramesOnly = base.KeyFramesOnly,
			StreamId = base.StreamId,
			EnableMulticast = base.MulticastEnabled,
			ConnectTimeout = base.ConnectTimeout,
			SendInitialImage = base.SendInitialImage,
			BitmapFormat = _bitmapFormat,
			Fps = base.FPS
		};
		return InternalMediaService.Instance.LiveVideoConnect(connectParams);
	}
}
