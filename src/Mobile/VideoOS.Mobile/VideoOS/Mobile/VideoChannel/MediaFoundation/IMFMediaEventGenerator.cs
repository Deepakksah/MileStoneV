using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[SuppressUnmanagedCodeSecurity]
[Guid("2CD0BD52-BCD5-4B89-B62C-EADC0C031E7D")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFMediaEventGenerator
{
	void GetEvent([In] MFEventFlag dwFlags, [MarshalAs(UnmanagedType.Interface)] out IMFMediaEvent ppEvent);

	void BeginGetEvent([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object o);

	void EndGetEvent(IMFAsyncResult pResult, out IMFMediaEvent ppEvent);

	void QueueEvent([In] MediaEventType met, [In][MarshalAs(UnmanagedType.LPStruct)] Guid guidExtendedType, [In] int hrStatus, [In][MarshalAs(UnmanagedType.LPStruct)] ConstPropVariant pvValue);
}
