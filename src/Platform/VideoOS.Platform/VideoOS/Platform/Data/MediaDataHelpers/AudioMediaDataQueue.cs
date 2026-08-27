using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Util.Pcm;

namespace VideoOS.Platform.Data.MediaDataHelpers;

public class AudioMediaDataQueue : MediaDataQueue
{
	public uint SampleRate { get; private set; }

	public uint BitsPerSample { get; private set; }

	public uint ChannelsCount { get; private set; }

	public AudioMediaDataQueue(uint sampleRate, uint bitsPerSample, uint channelsCount)
	{
		SampleRate = sampleRate;
		BitsPerSample = bitsPerSample;
		ChannelsCount = channelsCount;
		Reset();
	}

	public AudioMediaDataQueue()
		: this(8000u, 16u, 1u)
	{
	}

	private TimeSpan TimeSpanFromDataSize(int dataSize)
	{
		return TimeSpan.FromMilliseconds(PcmHelper.BytesCountToMilliseconds(dataSize, (int)SampleRate, (int)ChannelsCount, (int)(BitsPerSample / 8)));
	}

	public override MediaData GetMediaData(MediaProviderSession session)
	{
		byte[][] array = null;
		lock (base.MediaQueueLocker)
		{
			if (base.MediaQueue.Count > 0)
			{
				array = new byte[base.MediaQueue.Count][];
				base.MediaQueue.Select((MediaData mq) => mq.Bytes).ToArray().CopyTo(array, 0);
				base.MediaQueue.Clear();
			}
		}
		if (array != null)
		{
			byte[] array2 = array.Select((byte[] buf) => buf).Aggregate((byte[] b1, byte[] b2) => b1.Concat(b2).ToArray());
			DateTime lastSentDateTime = base.LastSentDateTime;
			TimeSpan timeSpan = TimeSpanFromDataSize(array2.Length);
			base.LastSentDateTime += timeSpan;
			return new MediaData
			{
				Bytes = array2,
				MediaContent = MediaContent.Pcm,
				DateTime = lastSentDateTime,
				TimeSpan = timeSpan
			};
		}
		return null;
	}

	protected override TimeSpan GetTimeSpan(IEnumerable<MediaData> buffers)
	{
		return TimeSpanFromDataSize(buffers.Sum((MediaData buff) => buff.Bytes.Length));
	}
}
