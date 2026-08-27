using System;
using VideoOS.Platform.Data;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Pcm;

namespace VideoOS.Mobile.AudioSource.Pcm;

public class OverlappingPcmDataListCorrector : IOverlappingPcmDataListCorrector
{
	private DateTime _lastPacketEndTime;

	private readonly Func<byte[], int, IPcmSourceSettings, byte[]> _corrector;

	public OverlappingPcmDataListCorrector(Func<byte[], int, IPcmSourceSettings, byte[]> corrector)
	{
		Reset();
		_corrector = corrector;
	}

	public PcmAudioDataList Do(PcmAudioDataList pcmAudioDataList)
	{
		if (pcmAudioDataList != null)
		{
			foreach (PcmAudioData data in pcmAudioDataList.DataList)
			{
				Do(data);
			}
		}
		return pcmAudioDataList;
	}

	public PcmAudioData Do(PcmAudioData pcmAudioData)
	{
		if (pcmAudioData != null)
		{
			if (_lastPacketEndTime != DateTime.MinValue && Math.Abs((pcmAudioData.DateTime - _lastPacketEndTime).TotalSeconds) < 2.0)
			{
				if (_lastPacketEndTime > pcmAudioData.DateTime)
				{
					CallTheCorrector(_lastPacketEndTime, pcmAudioData, _corrector);
				}
				else if (_lastPacketEndTime < pcmAudioData.DateTime)
				{
					pcmAudioData.DateTime = _lastPacketEndTime;
				}
			}
			_lastPacketEndTime = pcmAudioData.EndDateTime;
		}
		return pcmAudioData;
	}

	public static void CallTheCorrector(DateTime previousEndTime, PcmAudioData pcmAudioData, Func<byte[], int, IPcmSourceSettings, byte[]> corrector)
	{
		if (pcmAudioData != null && corrector != null)
		{
			pcmAudioData.Data = corrector(pcmAudioData.Data, PcmHelper.MillisecondsToBytesCount((int)(previousEndTime - pcmAudioData.DateTime).TotalMilliseconds, (int)pcmAudioData.PcmSourceSettings.SamplingRate, (int)pcmAudioData.PcmSourceSettings.Channels, (int)pcmAudioData.PcmSourceSettings.BitsPerSample >> 3), pcmAudioData.PcmSourceSettings);
			pcmAudioData.DateTime = previousEndTime;
		}
	}

	public void Reset()
	{
		_lastPacketEndTime = DateTime.MinValue;
	}
}
