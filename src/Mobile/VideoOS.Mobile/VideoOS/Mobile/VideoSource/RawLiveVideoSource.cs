using System;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Platform;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util.AdaptiveStreaming;

namespace VideoOS.Mobile.VideoSource;

public class RawLiveVideoSource : LiveStreamSource<VideoData>, IAdaptiveStreamingVideo
{
	private RawLiveSource _rawLiveSource;

	public IStreamSelectionParams StreamSelectionParams => _rawLiveSource?.StreamSelectionParams;

	public RawLiveVideoSource(Item camera, bool singleFrameQueue = true, bool sendInitialImage = false)
		: base(camera)
	{
		_rawLiveSource.SingleFrameQueue = singleFrameQueue;
		_rawLiveSource.SendInitialImage = sendInitialImage;
	}

	public override bool CreateRsChannel()
	{
		_rawLiveSource = new RawLiveSource(base.Item);
		_rawLiveSource.ConnectTimeout = LiveSourceConfiguration.Instance.ToolkitConnectTimeout;
		LiveSource = _rawLiveSource;
		return LiveSource != null;
	}

	public override bool CloseRsChannel()
	{
		_rawLiveSource = null;
		return base.CloseRsChannel();
	}

	protected override void LiveSourceContentEventHandler(object sender, EventArgs e)
	{
		if (e is LiveContentRawEventArgs e2)
		{
			if (e2.LiveContent != null)
			{
				QualityMeter.OnDataReceived((ulong)e2.LiveContent.Content.Count(), isFrame: true);
				_ = DateTime.UtcNow;
				base.OnData?.Invoke(new VideoData[1]
				{
					new VideoData
					{
						Content = e2.LiveContent.Content,
						StreamType = StreamParamsHelper.StreamType.Unknown,
						RawVideo = new RawVideoData
						{
							PrivacyMask = e2.LiveContent.PrivacyMask
						},
						Disposer = e2.LiveContent.Dispose
					}
				});
			}
			else if (e2.Exception != null)
			{
				Log.Exception(e2.Exception);
				LiveSourceContentExceptionEvent(this, e2.Exception);
			}
		}
		else if (e is LiveContentEventArgs { Exception: not null } e3)
		{
			Log.Exception(e3.Exception);
			LiveSourceContentExceptionEvent(this, e3.Exception);
		}
	}
}
