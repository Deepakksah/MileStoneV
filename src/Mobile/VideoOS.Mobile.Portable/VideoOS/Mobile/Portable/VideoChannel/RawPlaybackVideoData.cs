using System;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class RawPlaybackVideoData
{
	public bool HasMotion;

	public long SequenceNumber;

	public byte[] Signature;

	public DateTime NextDateTime;

	public DateTime PreviousDateTime;
}
