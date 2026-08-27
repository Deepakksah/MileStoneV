using System;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[SuppressUnmanagedCodeSecurity]
[Guid("0A97B3CF-8E7C-4A3D-8F8C-0C843DC247FB")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFStreamSink : IMFMediaEventGenerator
{
	new void GetEvent([In] MFEventFlag dwFlags, [MarshalAs(UnmanagedType.Interface)] out IMFMediaEvent ppEvent);

	new void BeginGetEvent([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object o);

	new void EndGetEvent(IMFAsyncResult pResult, out IMFMediaEvent ppEvent);

	new void QueueEvent([In] MediaEventType met, [In][MarshalAs(UnmanagedType.LPStruct)] Guid guidExtendedType, [In] int hrStatus, [In][MarshalAs(UnmanagedType.LPStruct)] ConstPropVariant pvValue);

	void GetMediaSink([MarshalAs(UnmanagedType.Interface)] out IMFMediaSink ppMediaSink);

	void GetIdentifier(out int pdwIdentifier);

	void GetMediaTypeHandler([MarshalAs(UnmanagedType.Interface)] out IMFMediaTypeHandler ppHandler);

	void ProcessSample([In][MarshalAs(UnmanagedType.Interface)] IMFSample pSample);

	void PlaceMarker([In] MFStreamSinkMarkerType eMarkerType, [In][MarshalAs(UnmanagedType.LPStruct)] ConstPropVariant pvarMarkerValue, [In][MarshalAs(UnmanagedType.LPStruct)] ConstPropVariant pvarContextValue);

	void Flush();
}
