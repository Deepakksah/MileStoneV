using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmDatabase.Utility;

[StructLayout(LayoutKind.Sequential, Size = 104)]
[UnsafeValueType]
[NativeCppClass]
public struct CmMediaStorageUtilityError
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum errorcode_t
	{

	}

	private long _003Calignment_0020member_003E;
}
