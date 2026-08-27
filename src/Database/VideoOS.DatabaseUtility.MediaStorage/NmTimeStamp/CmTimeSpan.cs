using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmTimeStamp;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public struct CmTimeSpan
{
	private long _003Calignment_0020member_003E;

	[SpecialName]
	public unsafe static void _003CMarshalDestroy_003E(CmTimeSpan* A_0)
	{
		global::_003CModule_003E.NmTimeStamp_002ECmTimeSpan_002E_007Bdtor_007D(A_0);
	}
}
