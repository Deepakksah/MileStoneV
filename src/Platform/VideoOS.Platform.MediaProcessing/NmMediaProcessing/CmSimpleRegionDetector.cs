using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 80)]
[NativeCppClass]
public struct CmSimpleRegionDetector
{
	[CLSCompliant(false)]
	[NativeCppClass]
	public enum maskFlag_t
	{

	}

	[StructLayout(LayoutKind.Sequential, Size = 16)]
	[NativeCppClass]
	[UnsafeValueType]
	[CLSCompliant(false)]
	public struct extentedMotionLevelInfo_t
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
