using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[ComImport]
[Guid("0000000c-0000-0000-C000-000000000046")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IStream
{
	[PreserveSig]
	HResult Read([Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbRead);

	[PreserveSig]
	HResult Write([MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] byte[] pv, int cb, IntPtr pcbWritten);

	[PreserveSig]
	HResult Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition);

	[PreserveSig]
	HResult SetSize(long libNewSize);

	HResult CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten);

	[PreserveSig]
	HResult Commit(int grfCommitFlags);

	[PreserveSig]
	HResult Revert();

	[PreserveSig]
	HResult LockRegion(long libOffset, long cb, int dwLockType);

	[PreserveSig]
	HResult UnlockRegion(long libOffset, long cb, int dwLockType);

	[PreserveSig]
	HResult Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag);

	[PreserveSig]
	HResult Clone(out IStream ppstm);
}
