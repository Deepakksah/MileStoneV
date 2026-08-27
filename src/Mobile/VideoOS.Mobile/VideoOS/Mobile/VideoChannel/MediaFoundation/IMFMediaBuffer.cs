using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("045FA593-8799-42B8-BC8D-8968C6453507")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFMediaBuffer
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Lock(out IntPtr ppbBuffer, out uint pcbMaxLength, out uint pcbCurrentLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void Unlock();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetCurrentLength(out uint pcbCurrentLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetCurrentLength([In] uint cbCurrentLength);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetMaxLength(out uint pcbMaxLength);
}
