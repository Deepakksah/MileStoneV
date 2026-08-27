using VideoOS.Platform;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource.LiveSource;

public class PcmLiveAudioSource : BasePcmLiveAudioSource
{
	public PcmLiveAudioSource(Item microphone, PcmSourceSettings pcmSourceSettings, IStreamSourceSettings streamSourceSettings)
		: base(microphone, pcmSourceSettings, streamSourceSettings)
	{
	}

	protected override void OnPcmData(byte[] pcmData)
	{
		NotifySubscribers(pcmData);
	}
}
