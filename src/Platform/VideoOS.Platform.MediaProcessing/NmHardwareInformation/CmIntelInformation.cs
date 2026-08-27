using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmHardwareInformation;

[StructLayout(LayoutKind.Sequential, Size = 40)]
[UnsafeValueType]
[NativeCppClass]
public struct CmIntelInformation
{
	private long _003Calignment_0020member_003E;
}
