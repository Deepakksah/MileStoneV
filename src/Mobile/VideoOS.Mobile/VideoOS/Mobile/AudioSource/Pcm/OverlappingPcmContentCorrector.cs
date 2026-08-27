using System;
using VideoOS.Platform.Live;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Pcm;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmContentCorrector : IOverlappingPcmContentCorrector
{
	private readonly PcmSourceSettings _pcmSourceSettings;

	private readonly Func<byte[], int, IPcmSourceSettings, byte[]> _actualCorrector;

	private DateTime _previousEndTime = DateTime.MinValue;

	public IPcmSourceSettings PcmSourceSettings => _pcmSourceSettings;

	public Action<LiveSourcePcmContent> OnCorrectedPcmContent { get; set; }

	public OverlappingPcmContentCorrector(PcmSourceSettings pcmSourceSettings, Func<byte[], int, IPcmSourceSettings, byte[]> actualCorrector)
	{
		_pcmSourceSettings = pcmSourceSettings;
		_actualCorrector = actualCorrector;
	}

	public void OnPcmContent(LiveSourcePcmContent pcmContent)
	{
		if (pcmContent == null)
		{
			return;
		}
		if (_previousEndTime != DateTime.MinValue)
		{
			if (_previousEndTime > pcmContent.BeginTime)
			{
				CallTheCorrector(_previousEndTime, pcmContent, _pcmSourceSettings, _actualCorrector);
			}
			else if (_previousEndTime < pcmContent.BeginTime && (pcmContent.BeginTime - _previousEndTime).TotalSeconds < 2.0)
			{
				AdjustNextSampleTime(_previousEndTime, pcmContent);
			}
		}
		_previousEndTime = pcmContent.EndTime;
		OnCorrectedPcmContent(pcmContent);
	}

	public static void CallTheCorrector(DateTime previousEndTime, LiveSourcePcmContent pcmContent, IPcmSourceSettings pcmSourceSettings, Func<byte[], int, IPcmSourceSettings, byte[]> corrector)
	{
		if (pcmContent != null && pcmSourceSettings != null && corrector != null)
		{
			pcmContent.Content = corrector(pcmContent.Content, PcmHelper.MillisecondsToBytesCount((int)(previousEndTime - pcmContent.BeginTime).TotalMilliseconds, (int)pcmSourceSettings.SamplingRate, (int)pcmSourceSettings.Channels, (int)pcmSourceSettings.BitsPerSample >> 3), pcmSourceSettings);
			pcmContent.BeginTime = previousEndTime;
		}
	}

	public static void AdjustNextSampleTime(DateTime previousEndTime, LiveSourcePcmContent next)
	{
		if (!(DateTime.MinValue == previousEndTime) && next != null)
		{
			TimeSpan timeSpan = next.EndTime - next.BeginTime;
			next.BeginTime = previousEndTime;
			next.EndTime = next.BeginTime + timeSpan;
		}
	}

	public LiveSourcePcmContent PopLast()
	{
		return null;
	}
}
