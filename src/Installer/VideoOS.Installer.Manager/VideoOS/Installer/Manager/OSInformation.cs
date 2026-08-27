using System;
using VideoOS.Installer.Configuration;

namespace VideoOS.Installer.Manager;

public class OSInformation
{
	public Version OSVersion { get; set; }

	public WinVersionTypeVersionsToExcludeProductType ProductType { get; set; }
}
