using System;

namespace VideoOS.Platform.Data;

public class MediaData
{
	public DateTime DateTime = DateTime.UtcNow;

	public TimeSpan TimeSpan = new TimeSpan(0L);

	public byte[] Bytes;

	public MediaContent MediaContent;
}
