using System;

namespace VideoOS.Installer.Common.Dism;

public class ProgressInfoEventArgs : EventArgs
{
	public uint Current { get; set; }

	public uint Total { get; set; }
}
