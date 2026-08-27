using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[SuppressUnmanagedCodeSecurity]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("AD4C1B00-4BF7-422F-9175-756693D9130D")]
public interface IMFByteStream
{
	uint GetCapabilities(out MFByteStreamCapabilities pdwCapabilities);

	uint GetLength(out long pqwLength);

	uint SetLength([In] long qwLength);

	uint GetCurrentPosition(out long pqwPosition);

	uint SetCurrentPosition([In] long qwPosition);

	uint IsEndOfStream([MarshalAs(UnmanagedType.Bool)] out bool pfEndOfStream);

	uint Read(IntPtr pb, [In] int cb, out int pcbRead);

	uint BeginRead(IntPtr pb, [In] int cb, [In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object pUnkState);

	uint EndRead([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncResult pResult, out int pcbRead);

	uint Write(IntPtr pb, [In] int cb, out int pcbWritten);

	uint BeginWrite(IntPtr pb, [In] int cb, [In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object pUnkState);

	uint EndWrite([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncResult pResult, out int pcbWritten);

	uint Seek([In] MFByteStreamSeekOrigin SeekOrigin, [In] long llSeekOffset, [In] MFByteStreamSeekingFlags dwSeekFlags, out long pqwCurrentPosition);

	uint Flush();

	uint Close();
}
