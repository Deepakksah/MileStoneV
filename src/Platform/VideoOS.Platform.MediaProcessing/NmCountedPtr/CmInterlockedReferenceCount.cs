using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmCountedPtr;

[StructLayout(LayoutKind.Sequential, Size = 4)]
[NativeCppClass]
public struct CmInterlockedReferenceCount
{
	private int _003Calignment_0020member_003E;
}
