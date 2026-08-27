using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Explicit)]
public class MediaSessionStartPosition
{
	[FieldOffset(0)]
	public short internalUse;

	[FieldOffset(8)]
	public long startPosition;

	public MediaSessionStartPosition(long startPosition)
	{
		internalUse = 20;
		this.startPosition = startPosition;
	}
}
