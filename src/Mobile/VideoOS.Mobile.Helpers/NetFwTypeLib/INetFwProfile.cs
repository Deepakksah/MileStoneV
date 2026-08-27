using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib;

[ComImport]
[CompilerGenerated]
[Guid("174A0DDA-E9F9-449D-993B-21AB667CA456")]
[TypeIdentifier]
public interface INetFwProfile
{
	void _VtblGap1_11();

	[DispId(8)]
	INetFwOpenPorts GloballyOpenPorts
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(8)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap2_1();

	[DispId(10)]
	INetFwAuthorizedApplications AuthorizedApplications
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
