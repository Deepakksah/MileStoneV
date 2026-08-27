using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("90377834-21D0-4DEE-8214-BA2E3E6C1127")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFMediaSession
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetEvent([In] uint dwFlags, [MarshalAs(UnmanagedType.Interface)] out IMFMediaEvent ppEvent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void BeginGetEvent([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object punkState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void EndGetEvent([In][MarshalAs(UnmanagedType.Interface)] IMFAsyncResult pResult, [MarshalAs(UnmanagedType.Interface)] out IMFMediaEvent ppEvent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void QueueEvent([In] uint met, [In][MarshalAs(UnmanagedType.LPStruct)] Guid guidExtendedType, [In][MarshalAs(UnmanagedType.Error)] int hrStatus, [In] ref object pvValue);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetTopology([In] uint dwSetTopologyFlags, [In][MarshalAs(UnmanagedType.Interface)] IMFTopology pTopology);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void ClearTopologies();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Start([In][MarshalAs(UnmanagedType.LPStruct)] Guid pguidTimeFormat, [In][MarshalAs(UnmanagedType.LPStruct)] MediaSessionStartPosition pvarStartPosition);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Pause();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Stop();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Shutdown();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetClock([MarshalAs(UnmanagedType.Interface)] out IMFClock ppClock);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetSessionCapabilities(out uint pdwCaps);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetFullTopology([In] uint dwGetFullTopologyFlags, [In] ulong TopoId, [MarshalAs(UnmanagedType.Interface)] out IMFTopology ppFullTopology);
}
