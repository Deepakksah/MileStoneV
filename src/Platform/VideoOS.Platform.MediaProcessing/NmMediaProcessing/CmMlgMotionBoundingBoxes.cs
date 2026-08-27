using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 48)]
[NativeCppClass]
public struct CmMlgMotionBoundingBoxes
{
	[StructLayout(LayoutKind.Sequential, Size = 16)]
	[NativeCppClass]
	[CLSCompliant(false)]
	[UnsafeValueType]
	public struct SmBoundingBox
	{
		private int _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
