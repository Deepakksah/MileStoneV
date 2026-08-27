using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public static struct ImAudioToolkit
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum state_t
	{

	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public static struct ImAudioHandler
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public static struct ImOutOfBandHandler
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public static struct ImStateHandler
	{
		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
