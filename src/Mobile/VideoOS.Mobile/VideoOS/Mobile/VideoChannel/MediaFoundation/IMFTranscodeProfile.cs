using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("4ADFDBA3-7AB0-4953-A62B-461E7FF3DA1E")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFTranscodeProfile
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetAudioAttributes([In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pAttrs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetAudioAttributes([MarshalAs(UnmanagedType.Interface)] out IMFAttributes ppAttrs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetVideoAttributes([In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pAttrs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetVideoAttributes([MarshalAs(UnmanagedType.Interface)] out IMFAttributes ppAttrs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void SetContainerAttributes([In][MarshalAs(UnmanagedType.Interface)] IMFAttributes pAttrs);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetContainerAttributes([MarshalAs(UnmanagedType.Interface)] out IMFAttributes ppAttrs);
}
