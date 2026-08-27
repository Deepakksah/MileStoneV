using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("FBE5A32D-A497-4B61-BB85-97B1A848A6E3")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFSourceResolver
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CreateObjectFromURL([In][MarshalAs(UnmanagedType.LPWStr)] string pwszURL, [In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pProps, out uint pObjectType, [MarshalAs(UnmanagedType.IUnknown)] out object ppObject);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CreateObjectFromByteStream([In][MarshalAs(UnmanagedType.Interface)] IMFByteStream pByteStream, [In][MarshalAs(UnmanagedType.LPWStr)] string pwszURL, [In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pProps, out uint pObjectType, [MarshalAs(UnmanagedType.IUnknown)] out object ppObject);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void BeginCreateObjectFromURL([In][MarshalAs(UnmanagedType.LPWStr)] string pwszURL, [In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pProps, [MarshalAs(UnmanagedType.Interface)] out object ppIUnknownCancelCookie, [In][MarshalAs(UnmanagedType.Interface)] IMFAsyncCallback pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object punkState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void EndCreateObjectFromURL([In][MarshalAs(UnmanagedType.IUnknown)] object pResult, out uint pObjectType, [MarshalAs(UnmanagedType.IUnknown)] out object ppObject);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void BeginCreateObjectFromByteStream([In][MarshalAs(UnmanagedType.Interface)] object pByteStream, [In][MarshalAs(UnmanagedType.LPWStr)] string pwszURL, [In] uint dwFlags, [In][MarshalAs(UnmanagedType.Interface)] IPropertyStore pProps, [MarshalAs(UnmanagedType.IUnknown)] out IMFAsyncCallback ppIUnknownCancelCookie, [In][MarshalAs(UnmanagedType.Interface)] object pCallback, [In][MarshalAs(UnmanagedType.IUnknown)] object punkState);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void EndCreateObjectFromByteStream([In][MarshalAs(UnmanagedType.IUnknown)] object pResult, out uint pObjectType, [MarshalAs(UnmanagedType.IUnknown)] out object ppObject);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void CancelObjectCreation([In][MarshalAs(UnmanagedType.IUnknown)] object pIUnknownCancelCookie);
}
