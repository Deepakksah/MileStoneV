using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

public struct MFTOutputDataBuffer
{
	public int dwStreamID;

	public IntPtr pSample;

	public MFTOutputDataBufferFlags dwStatus;

	[MarshalAs(UnmanagedType.Interface)]
	public IMFCollection pEvents;
}
