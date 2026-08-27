using System;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities.Rects;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Platform;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util.AdaptiveStreaming;

namespace VideoOS.Mobile.VideoSource;

public class JpegLiveVideoSource : LiveStreamSource<VideoData>, IRsTranscodedVideo, IAdaptiveStreamingVideo
{
	private JPEGLiveSource _jpegLiveSource;

	public IStreamSelectionParams StreamSelectionParams => _jpegLiveSource?.StreamSelectionParams;

	public JpegLiveVideoSource(Item camera, bool allowUpscaling, VideoParams videoParams)
		: base(camera)
	{
		if (_jpegLiveSource != null)
		{
			_jpegLiveSource.SetKeepAspectRatio(keepAspectRatio: true, fillSpace: false);
			SetQuality((int)videoParams.CompressionLvl);
			_jpegLiveSource.KeyFramesOnly = videoParams.KeyFramesOnly;
			_jpegLiveSource.AllowUpscaling = allowUpscaling;
		}
	}

	protected override void LiveSourceContentEventHandler(object sender, EventArgs e)
	{
		if (!(e is LiveContentEventArgs e2))
		{
			return;
		}
		if (e2.LiveContent != null)
		{
			QualityMeter.OnDataReceived((ulong)e2.LiveContent.Content.Count(), isFrame: true);
			if (base.OnData != null)
			{
				base.OnData(new VideoData[1]
				{
					new VideoData
					{
						BeginTimeUtc = e2.LiveContent.BeginTime,
						EndTimeUtc = e2.LiveContent.EndTime,
						Content = e2.LiveContent.Content,
						StreamType = StreamParamsHelper.StreamType.Transcoded,
						Transcoded = new TranscodedVideoData
						{
							Width = e2.LiveContent.Width,
							Height = e2.LiveContent.Height,
							CroppingDefined = e2.LiveContent.CroppingDefined,
							CroppingRectangle = new FloatRectExt(e2.LiveContent.CropX, e2.LiveContent.CropY, e2.LiveContent.CropWidth, e2.LiveContent.CropHeight, 0),
							CustomData = e2.LiveContent.CustomData,
							HardwareDecodingStatus = e2.LiveContent.HardwareDecodingStatus
						},
						Disposer = e2.LiveContent.Dispose,
						ContainsKeyFrame = true
					}
				});
			}
		}
		else if (e2.Exception != null)
		{
			Log.Exception(e2.Exception);
			LiveSourceContentExceptionEvent(this, e2.Exception);
		}
	}

	public override bool CreateRsChannel()
	{
		_jpegLiveSource = new JPEGLiveSource(base.Item);
		_jpegLiveSource.ConnectTimeout = LiveSourceConfiguration.Instance.ToolkitConnectTimeout;
		LiveSource = _jpegLiveSource;
		return LiveSource != null;
	}

	public override bool CloseRsChannel()
	{
		_jpegLiveSource = null;
		return base.CloseRsChannel();
	}

	public void BeginParamsChange()
	{
		_jpegLiveSource?.SuspendSettingsUpdate();
	}

	public void SetWidthHeight(int width, int height)
	{
		if (_jpegLiveSource != null)
		{
			_jpegLiveSource.Width = width;
			_jpegLiveSource.Height = height;
			_jpegLiveSource.SetWidthHeight();
		}
	}

	public void SetCropping(double left, double top, double width, double height)
	{
		_jpegLiveSource?.SetCropRectangle(left, top, width, height);
	}

	public void SetQuality(int quality)
	{
		if (_jpegLiveSource != null)
		{
			_jpegLiveSource.Compression = quality;
		}
	}

	public void SetFPS(int fps)
	{
		if (_jpegLiveSource != null)
		{
			_jpegLiveSource.FPS = fps;
		}
	}

	public void SetCustomData(object data)
	{
		if (_jpegLiveSource != null)
		{
			_jpegLiveSource.CustomData = data;
		}
	}

	public void EndParamsChange()
	{
		_jpegLiveSource?.ResumeSettingsUpdate();
	}
}
