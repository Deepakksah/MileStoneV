using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using SharpDX.MediaFoundation;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.VideoChannel.MediaFoundation;

namespace VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Classes;

public static class MediaBufferHelper
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IMFMediaBuffer CreateMediaBuffer(byte[] pFrame)
	{
		return CreateMediaBuffer(new ByteBuffer(pFrame, 0, pFrame.Length));
	}

	public static IMFMediaBuffer CreateMediaBuffer(ByteBuffer frameBuffer)
	{
		IMFMediaBuffer iMFMediaBuffer = MFHelper.MFCreateMemoryBuffer((uint)frameBuffer.lenWritten);
		iMFMediaBuffer.Lock(out var ppbBuffer, out var _, out var _);
		Marshal.Copy(frameBuffer.data, frameBuffer.begin, ppbBuffer, frameBuffer.lenWritten);
		iMFMediaBuffer.Unlock();
		iMFMediaBuffer.SetCurrentLength((uint)frameBuffer.lenWritten);
		return iMFMediaBuffer;
	}

	public static void PutDataIntoMediaBuffer(MediaBuffer mediaBuffer, byte[] dataBytes)
	{
		int num = default(int);
		int num2 = default(int);
		IntPtr destination = mediaBuffer.Lock(ref num, ref num2);
		Marshal.Copy(dataBytes, 0, destination, dataBytes.Length);
		mediaBuffer.CurrentLength = dataBytes.Length;
		mediaBuffer.Unlock();
	}
}
