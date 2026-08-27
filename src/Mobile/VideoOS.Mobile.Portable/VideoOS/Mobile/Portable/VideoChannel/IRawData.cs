using System;

namespace VideoOS.Mobile.Portable.VideoChannel;

public interface IRawData
{
	byte[] Content { get; }

	Action Disposer { get; }
}
