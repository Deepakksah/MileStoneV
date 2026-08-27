using System;
using System.Globalization;

namespace VideoOS.Installer.Common;

public class CultureChangedEventArgs : EventArgs
{
	public CultureInfo Culture { get; set; }

	public CultureChangedEventArgs(CultureInfo culture)
	{
		Culture = culture;
	}
}
