using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate int FdiFileCloseDelegate(IntPtr hf);
