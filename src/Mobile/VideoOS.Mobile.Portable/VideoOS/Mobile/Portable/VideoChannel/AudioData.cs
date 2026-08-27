using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class AudioData : StreamData
{
	public AudioData()
	{
	}

	public AudioData(byte[] byteData)
	{
		base.EndTimeUtc = (base.BeginTimeUtc = DateTime.UtcNow);
		base.Content = byteData;
		base.StreamType = StreamParamsHelper.StreamType.Transcoded;
		base.Disposer = null;
	}
}
