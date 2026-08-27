using VideoOS.Mobile.Portable.VideoChannel;
using VideoOS.Mobile.VideoSource;
using VideoOS.Platform.Util;

namespace VideoOS.Mobile.AudioSource;

public class AudioPcmMixerRsStream : RsStreamMuxer<AudioData>
{
	public AudioPcmMixerRsStream(IPcmSourceSettings pcmSourceSettings)
		: base((IRsStreamMuxerImplementer<AudioData>)new AudioPcmMixerImplementer(pcmSourceSettings))
	{
	}
}
