using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("ac6b7889-0740-4d51-8619-905994a55cc6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFAsyncResult
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetState([MarshalAs(UnmanagedType.IUnknown)] out object ppunkState);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	int GetStatus();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetStatus([In][MarshalAs(UnmanagedType.Error)] int hrStatus);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetObject([MarshalAs(UnmanagedType.IUnknown)] out object ppObject);
}
