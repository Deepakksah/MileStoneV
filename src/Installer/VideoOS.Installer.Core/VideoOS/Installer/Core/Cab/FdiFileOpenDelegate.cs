using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr FdiFileOpenDelegate(string fileName, int oflag, int pmode);
