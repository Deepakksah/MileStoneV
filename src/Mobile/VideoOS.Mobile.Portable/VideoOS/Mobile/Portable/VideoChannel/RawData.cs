using System;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class RawData : IRawData
{
	public byte[] Content { get; set; }

	public Action Disposer { get; set; }
}
