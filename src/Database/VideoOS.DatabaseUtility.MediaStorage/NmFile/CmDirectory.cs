using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmFile;

[StructLayout(LayoutKind.Sequential, Size = 40)]
[NativeCppClass]
[UnsafeValueType]
public struct CmDirectory
{
	private long _003Calignment_0020member_003E;
}
