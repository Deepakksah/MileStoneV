using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 32)]
[NativeCppClass]
public static struct ImPlaybackSourceToolkit
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum move_status_t
	{

	}

	[CLSCompliant(false)]
	[NativeCppClass]
	public enum search_criteria_t
	{

	}

	[NativeCppClass]
	[CLSCompliant(false)]
	public enum get_data_status_t
	{

	}

	private long _003Calignment_0020member_003E;
}
