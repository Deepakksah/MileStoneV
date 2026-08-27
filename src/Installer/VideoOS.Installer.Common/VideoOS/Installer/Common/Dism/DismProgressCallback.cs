using System;

namespace VideoOS.Installer.Common.Dism;

public delegate void DismProgressCallback(uint current, uint total, IntPtr userData);
