using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 32)]
[NativeCppClass]
public struct CmMotionDetectionResult
{
	[StructLayout(LayoutKind.Sequential, Size = 24)]
	[CLSCompliant(false)]
	[NativeCppClass]
	public struct SmMotionDetectionEvent
	{
		[CLSCompliant(false)]
		[NativeCppClass]
		public enum eventType_t
		{

		}

		private long _003Calignment_0020member_003E;
	}

	[CLSCompliant(false)]
	[NativeCppClass]
	public enum decoderImplementation_t
	{

	}

	private long _003Calignment_0020member_003E;
}
