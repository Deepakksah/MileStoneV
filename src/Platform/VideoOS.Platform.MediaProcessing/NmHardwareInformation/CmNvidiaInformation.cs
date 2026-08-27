using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmHardwareInformation;

[StructLayout(LayoutKind.Sequential, Size = 16)]
[NativeCppClass]
public struct CmNvidiaInformation
{
	private long _003Calignment_0020member_003E;
}
