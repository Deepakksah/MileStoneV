using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 40)]
[NativeCppClass]
public struct CmMotionHistogramGrid
{
	[StructLayout(LayoutKind.Sequential, Size = 1024)]
	[UnsafeValueType]
	[NativeCppClass]
	[CLSCompliant(false)]
	public struct MotionHistogram
	{
		private int _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
