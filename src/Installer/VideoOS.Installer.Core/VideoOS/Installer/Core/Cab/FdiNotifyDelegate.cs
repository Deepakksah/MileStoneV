using System;
using System.Runtime.InteropServices;

namespace VideoOS.Installer.Core.Cab;

[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
public delegate IntPtr FdiNotifyDelegate(CabInvoke.FDINotificationType fdint, [MarshalAs(UnmanagedType.LPStruct)] FDINotification fdin);
