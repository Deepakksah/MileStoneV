using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("5BC8A76B-869A-46A3-9B03-FA218A66AEBE")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMFCollection
{
	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetElementCount(out uint pcElements);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void GetElement([In] uint dwElementIndex, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnkElement);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void AddElement([In][MarshalAs(UnmanagedType.IUnknown)] object pUnkElement);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void RemoveElement([In] uint dwElementIndex, [MarshalAs(UnmanagedType.IUnknown)] out object ppUnkElement);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void InsertElementAt([In] uint dwIndex, [In][MarshalAs(UnmanagedType.IUnknown)] object pUnknown);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	void RemoveAllElements();
}
