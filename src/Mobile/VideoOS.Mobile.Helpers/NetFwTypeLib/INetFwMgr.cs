using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NetFwTypeLib;

[ComImport]
[CompilerGenerated]
[Guid("F7898AF5-CAC4-4632-A2EC-DA06E5111AF2")]
[TypeIdentifier]
public interface INetFwMgr
{
	[DispId(1)]
	INetFwPolicy LocalPolicy
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}
}
