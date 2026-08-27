using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class RawLiveSource : BaseLiveSource<LiveSourceRawContent, LiveContentRawEventArgs>
{
	public bool RequestSignatures { get; set; }

	public bool EnableMulticast
	{
		get
		{
			return base.MulticastEnabled;
		}
		set
		{
			base.MulticastEnabled = value;
		}
	}

	public RawLiveSource(Item item)
		: base(item, (IEnumerable<Guid>)new Guid[4]
		{
			Kind.Camera,
			Kind.Microphone,
			Kind.Speaker,
			Kind.Metadata
		}, true)
	{
		RequestSignatures = false;
		new TrackPageViewData(typeof(RawLiveSource)).Send();
	}

	protected override void OnUpdateLiveStart()
	{
	}

	protected override void OnUpdateSettings()
	{
	}

	protected override void HandleLiveDataContent(LiveSourceRawContent data)
	{
		if (data != null)
		{
			data.RawLiveSource = this;
			LiveContentRawEventArgs eventArgs = new LiveContentRawEventArgs
			{
				LiveContent = data
			};
			if (!OnLiveContentEvent(eventArgs))
			{
				data.Dispose();
			}
		}
	}

	protected override object ConnectLiveSource(string media)
	{
		InternalMediaService.LiveVideoRawConnectParams connectParams = new InternalMediaService.LiveVideoRawConnectParams
		{
			DeviceFqid = Item.FQID,
			StreamId = base.StreamId,
			Media = media,
			RequestSignatures = RequestSignatures,
			EnableMulticast = base.MulticastEnabled,
			ConnectTimeout = base.ConnectTimeout,
			SendInitialImage = base.SendInitialImage
		};
		return InternalMediaService.Instance.LiveVideoRawConnect(connectParams);
	}

	public override EventArgs CreateExceptionEventArgs(Exception ex)
	{
		return new LiveContentRawEventArgs
		{
			Exception = ex
		};
	}
}
