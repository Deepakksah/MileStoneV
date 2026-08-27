using System;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public class PcmMediaProviderChannel : MediaProviderChannel
{
	private static readonly byte[] NoSoundData;

	private const int NoSoundDataLengthMs = 100;

	static PcmMediaProviderChannel()
	{
		NoSoundData = new byte[160];
	}

	public PcmMediaProviderChannel(int channel)
		: base(channel)
	{
	}

	protected override MediaData GetMediaDataInIdle()
	{
		return new MediaData
		{
			Bytes = NoSoundData,
			DateTime = DateTime.UtcNow,
			TimeSpan = TimeSpan.FromMilliseconds(100.0),
			MediaContent = MediaContent.Pcm
		};
	}
}
