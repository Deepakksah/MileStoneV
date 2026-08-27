using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.VideoChannel.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Encoding.SharpDX;

public static class MediaBufferExtensions
{
	public static byte[] CopyData(this MediaBuffer mediaBuffer)
	{
		(Marshal.GetObjectForIUnknown(((CppObject)mediaBuffer).NativePointer) as IMFMediaBuffer).Lock(out var ppbBuffer, out var pcbMaxLength, out var pcbCurrentLength);
		int num = (int)Math.Min(pcbMaxLength, pcbCurrentLength);
		byte[] array = new byte[num];
		Marshal.Copy(ppbBuffer, array, 0, num);
		mediaBuffer.Unlock();
		return array;
	}
}
