using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 64)]
[NativeCppClass]
public struct CmSimpleImageRegion
{
	[StructLayout(LayoutKind.Sequential, Size = 16)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct CmPosition
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
