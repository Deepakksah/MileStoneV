using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public static struct ImImage
{
	[CLSCompliant(false)]
	[NativeCppClass]
	public enum image_format_t
	{

	}

	[NativeCppClass]
	[CLSCompliant(false)]
	public enum color_space_detail_t
	{

	}

	private long _003Calignment_0020member_003E;
}
