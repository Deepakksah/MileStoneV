using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public static struct ImImageMemoryAllocation
{
	[StructLayout(LayoutKind.Sequential, Size = 48)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public struct CmPlane
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 48)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct CmWriteablePlane
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
