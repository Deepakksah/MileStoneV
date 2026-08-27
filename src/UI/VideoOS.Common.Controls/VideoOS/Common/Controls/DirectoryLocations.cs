using System;

namespace VideoOS.Common.Controls;

[Flags]
public enum DirectoryLocations
{
	None = 0,
	LocalComputer = 1,
	JoinedDomain = 2,
	EnterpriseDomain = 4,
	GlobalCatalog = 8,
	ExternalDomain = 0x10,
	Workgroup = 0x20,
	UserEntered = 0x40,
	All = 0x7F
}
