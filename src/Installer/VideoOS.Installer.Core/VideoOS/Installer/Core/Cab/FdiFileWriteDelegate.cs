using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int FdiFileWriteDelegate(IntPtr hf, [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] buffer, int cb);
