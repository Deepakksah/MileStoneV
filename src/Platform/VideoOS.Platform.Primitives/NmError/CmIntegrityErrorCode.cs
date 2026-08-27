using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace NmError;

[StructLayout(LayoutKind.Sequential, Size = 1)]
[NativeCppClass]
public struct CmIntegrityErrorCode
{
	[NativeCppClass]
	[CLSCompliant(false)]
	public enum errorcode_t
	{

	}
}
