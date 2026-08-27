using System;
using System.Linq;
using VideoOS.Mobile.AudioSource.Pcm;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.VideoSource;
using VideoOS.Platform;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.LiveSource;

public abstract class BasePcmLiveAudioSource : LiveStreamSource<AudioData>
{
	private PcmLiveSource _pcmLiveSource;

	private IOverlappingPcmContentCorrector _contentCorrector;

	private readonly IStreamSourceSettings _streamSourceSettings;

	protected PcmSourceSettings PcmSourceSettings { get; private set; }

	protected BasePcmLiveAudioSource(Item microphone, PcmSourceSettings pcmSourceSettings, IStreamSourceSettings streamSourceSettings)
		: base(microphone)
	{
		PcmSourceSettings = pcmSourceSettings;
		SetPcmSourceSettings();
		_streamSourceSettings = streamSourceSettings;
		_contentCorrector = StreamSourceSettingsHelper.CreatePcmContentCorrector(_streamSourceSettings, _pcmLiveSource?.PcmSourceSettings);
		IOverlappingPcmContentCorrector contentCorrector = _contentCorrector;
		contentCorrector.OnCorrectedPcmContent = (Action<LiveSourcePcmContent>)Delegate.Combine(contentCorrector.OnCorrectedPcmContent, new Action<LiveSourcePcmContent>(OnCorrectedPcmContent));
	}

	protected abstract void OnPcmData(byte[] pcmData);

	protected void NotifySubscribers(byte[] byteData)
	{
		DateTime utcNow = DateTime.UtcNow;
		base.OnData?.Invoke(new AudioData[1]
		{
			new AudioData
			{
				BeginTimeUtc = utcNow,
				EndTimeUtc = utcNow,
				Content = byteData,
				StreamType = StreamParamsHelper.StreamType.Transcoded,
				Disposer = null
			}
		});
	}

	private void SetPcmSourceSettings()
	{
		if (_pcmLiveSource != null && PcmSourceSettings != null)
		{
			_pcmLiveSource.PcmSourceSettings.BitsPerSample = PcmSourceSettings.BitsPerSample;
			_pcmLiveSource.PcmSourceSettings.Channels = PcmSourceSettings.Channels;
			_pcmLiveSource.PcmSourceSettings.SamplingRate = PcmSourceSettings.SamplingRate;
		}
	}

	public override bool CreateRsChannel()
	{
		_pcmLiveSource = new PcmLiveSource(base.Item);
		_pcmLiveSource.ConnectTimeout = LiveSourceConfiguration.Instance.ToolkitConnectTimeout;
		SetPcmSourceSettings();
		LiveSource = _pcmLiveSource;
		return LiveSource != null;
	}

	public override bool CloseRsChannel()
	{
		_pcmLiveSource = null;
		bool result = base.CloseRsChannel();
		CloseContentCorrector();
		return result;
	}

	private void CloseContentCorrector()
	{
		if (_contentCorrector != null)
		{
			IOverlappingPcmContentCorrector contentCorrector = _contentCorrector;
			contentCorrector.OnCorrectedPcmContent = (Action<LiveSourcePcmContent>)Delegate.Remove(contentCorrector.OnCorrectedPcmContent, new Action<LiveSourcePcmContent>(OnCorrectedPcmContent));
		}
		_contentCorrector = null;
	}

	protected override void LiveSourceContentEventHandler(object sender, EventArgs e)
	{
		if (e is LiveContentPcmEventArgs e2)
		{
			if (e2.LiveContent != null)
			{
				QualityMeter.OnDataReceived((ulong)e2.LiveContent.Content.Count(), isFrame: true);
				_contentCorrector.OnPcmContent(e2.LiveContent);
			}
			else if (e2.Exception != null)
			{
				Log.Exception(e2.Exception);
				LiveSourceContentExceptionEvent(this, e2.Exception);
			}
		}
		else
		{
			LiveContentEventArgs e3 = e as LiveContentEventArgs;
			if (e3?.Exception != null)
			{
				Log.Exception(e3.Exception);
				LiveSourceContentExceptionEvent(this, e3.Exception);
			}
		}
	}

	private void OnCorrectedPcmContent(LiveSourcePcmContent liveSourcePcmContent)
	{
		if (liveSourcePcmContent != null)
		{
			OnPcmData(liveSourcePcmContent.Content);
			liveSourcePcmContent.Dispose();
		}
	}
}
