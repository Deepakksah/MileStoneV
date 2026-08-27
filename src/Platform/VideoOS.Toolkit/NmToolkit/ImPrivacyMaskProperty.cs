using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 24)]
[NativeCppClass]
public static struct ImPrivacyMaskProperty
{
	[CLSCompliant(false)]
	[NativeCppClass]
	public enum cell_privacy_t : byte
	{

	}

	[CLSCompliant(false)]
	[NativeCppClass]
	public enum mask_size_t : byte
	{

	}

	[StructLayout(LayoutKind.Sequential, Size = 3)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct SmSolidCellParameters
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct SmBlurCellParameters
	{
	}

	[StructLayout(LayoutKind.Sequential, Size = 5)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public struct SmCellData
	{
		[StructLayout(LayoutKind.Explicit, Size = 3)]
		[CLSCompliant(false)]
		[NativeCppClass]
		public struct _003Cunnamed_002Dtype_002DParameters_003E
		{
		}
	}

	private long _003Calignment_0020member_003E;
}
