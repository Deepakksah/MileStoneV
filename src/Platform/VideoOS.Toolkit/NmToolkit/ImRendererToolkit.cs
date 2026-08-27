using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmToolkit;

[StructLayout(LayoutKind.Sequential, Size = 8)]
[NativeCppClass]
public static struct ImRendererToolkit
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum source_state_t
	{

	}

	[CLSCompliant(false)]
	[NativeCppClass]
	public enum hardware_acceleration_t
	{

	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct ImRenderingParameters
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 56)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public struct SmImageRenderingParameters
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public static struct ImImageRenderingInformation
	{
		[NativeCppClass]
		[CLSCompliant(false)]
		public enum image_model_t
		{

		}

		[CLSCompliant(false)]
		[NativeCppClass]
		public enum eDataAvailabilityAtRequestedTime
		{

		}

		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public static struct ImBGR32ImageRenderingInformation
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public static struct ImD3D9SurfaceRenderingInformation
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[NativeCppClass]
	[CLSCompliant(false)]
	public static struct ImRenderingHandler
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
	public static struct ImSourceStateHandler
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public static struct ImDiagnosticsHandler
	{
		private long _003Calignment_0020member_003E;
	}

	[StructLayout(LayoutKind.Sequential, Size = 8)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public static struct ImDiagnosticsKeyInfo
	{
		[CLSCompliant(false)]
		[NativeCppClass]
		public enum ePerformanceCounterType
		{

		}

		private long _003Calignment_0020member_003E;
	}

	private long _003Calignment_0020member_003E;
}
