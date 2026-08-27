using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmError;

[StructLayout(LayoutKind.Sequential, Size = 96)]
[CLSCompliant(false)]
[NativeCppClass]
[UnsafeValueType]
public struct CmError
{
	private long _003Calignment_0020member_003E;
}
