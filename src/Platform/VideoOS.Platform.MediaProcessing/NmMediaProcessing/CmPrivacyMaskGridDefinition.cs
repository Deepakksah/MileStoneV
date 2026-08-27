using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 56)]
[NativeCppClass]
public struct CmPrivacyMaskGridDefinition
{
	[StructLayout(LayoutKind.Sequential, Size = 32)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public struct Grid
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
