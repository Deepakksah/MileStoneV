using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class PcmLiveSource : BaseLiveSource<LiveSourceContent, LiveContentPcmEventArgs>
{
	private bool _settingsChanged;

	public PcmSourceSettings PcmSourceSettings { get; private set; }

	public PcmLiveSource(Item item)
		: base(item, (IEnumerable<Guid>)new Guid[2]
		{
			Kind.Microphone,
			Kind.Speaker
		}, false)
	{
		PcmSourceSettings = new PcmSourceSettings(OnSettingsChange);
		new TrackPageViewData(typeof(PcmLiveSource)).Send();
	}

	private void OnSettingsChange(PcmSourceSettings pcmSourceSettings)
	{
		_settingsChanged = true;
	}

	protected override void OnUpdateLiveStart()
	{
	}

	protected override void OnUpdateSettings()
	{
		if (_settingsChanged)
		{
			_settingsChanged = false;
			InternalMediaService.Instance.SetPcmParameters(base.SessionId, (uint)PcmSourceSettings.Channels, PcmSourceSettings.SamplingRate, (uint)PcmSourceSettings.BitsPerSample);
		}
	}

	protected override void HandleLiveDataContent(LiveSourceContent data)
	{
		if (data != null)
		{
			LiveContentPcmEventArgs e = new LiveContentPcmEventArgs
			{
				LiveContent = new LiveSourcePcmContent()
			};
			e.LiveContent.Content = data.Content;
			e.LiveContent.BeginTime = data.BeginTime;
			e.LiveContent.EndTime = data.EndTime;
			e.LiveContent.PcmLiveSource = this;
			e.LiveContent.CorrectTimeStamps();
			if (!OnLiveContentEvent(e))
			{
				e.LiveContent.Dispose();
			}
		}
	}

	protected override object ConnectLiveSource(string media)
	{
		return InternalMediaService.Instance.LiveAudioConnect(new InternalMediaService.PcmAudioConnectParams(PcmSourceSettings)
		{
			DeviceFqid = Item.FQID,
			ConnectTimeout = base.ConnectTimeout
		});
	}

	public override EventArgs CreateExceptionEventArgs(Exception ex)
	{
		return new LiveContentPcmEventArgs
		{
			Exception = ex
		};
	}
}
