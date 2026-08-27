using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmMediaProcessing;

[StructLayout(LayoutKind.Sequential, Size = 20)]
[NativeCppClass]
public struct CmByteDataHeaderAnalysisResult
{
	[CLSCompliant(false)]
	[NativeCppClass]
	public enum e_dataType_t
	{

	}

	[CLSCompliant(false)]
	[NativeCppClass]
	public enum e_subsamplingCategory_t
	{

	}

	private int _003Calignment_0020member_003E;
}
