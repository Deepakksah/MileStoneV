using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("3137f1cd-fe5e-4805-a5d8-fb477448cb3d")]
[SuppressUnmanagedCodeSecurity]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFSinkWriter
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void AddStream([In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pTargetMediaType, out uint pdwStreamIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetInputMediaType([In] uint dwStreamIndex, [In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pInputMediaType, [In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pEncodingParameters);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void BeginWriting();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void WriteSample([In] uint dwStreamIndex, [In][MarshalAs(UnmanagedType.Interface)] IMFSample pSample);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SendStreamTick([In] uint dwStreamIndex, [In] ulong llTimestamp);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void PlaceMarker([In] uint dwStreamIndex, [In] IntPtr pvContext);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void NotifyEndOfSegment([In] uint dwStreamIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Flush([In] uint dwStreamIndex);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void DoFinalize();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetServiceForStream([In] uint dwStreamIndex, [In] ref Guid guidService, [In] ref Guid riid, out IntPtr ppvObject);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetStatistics([In] uint dwStreamIndex, out object pStats);
}
