using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 80)]
[NativeCppClass]
public struct CmStreamMotionDetectionParameters
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum detectionScope_t
	{

	}

	[NativeCppClass]
	[CLSCompliant(false)]
	public enum detectionResolution_t
	{

	}

	[NativeCppClass]
	[CLSCompliant(false)]
	public enum decoderMode_t
	{

	}

	private long _003Calignment_0020member_003E;
}
