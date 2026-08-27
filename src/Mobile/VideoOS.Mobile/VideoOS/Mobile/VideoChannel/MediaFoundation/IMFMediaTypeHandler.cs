using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("E93DCF6C-4B07-4E1E-8123-AA16ED6EADF5")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFMediaTypeHandler
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void IsMediaTypeSupported([In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pMediaType, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetMediaTypeCount(out uint pdwTypeCount);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetMediaTypeByIndex([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IMFMediaType ppType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetCurrentMediaType([In][MarshalAs(UnmanagedType.Interface)] IMFMediaType pMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetCurrentMediaType(out IMFMediaType pMediaType);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetMajorType(out Guid pguidMajorType);
}
