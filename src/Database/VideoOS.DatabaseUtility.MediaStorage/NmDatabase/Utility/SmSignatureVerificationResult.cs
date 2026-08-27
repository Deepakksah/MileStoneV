using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmDatabase.Utility;

[StructLayout(LayoutKind.Sequential, Size = 48)]
[NativeCppClass]
public struct SmSignatureVerificationResult
{
	[StructLayout(LayoutKind.Sequential, Size = 24)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct SmResultItem
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
