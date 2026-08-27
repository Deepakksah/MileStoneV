using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public static struct ImDigitalZoomController
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum eMoveCommand
	{

	}

	[StructLayout(LayoutKind.Sequential, Size = 16)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct Point
	{
		private double _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
