using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int FdiFileReadDelegate(IntPtr hf, [In][Out][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)] byte[] buffer, int cb);
