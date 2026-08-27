using System;

namespace VideoOS.Common.Controls;

[Flags]
public enum DirectoryObjectTypes
{
	None = 0,
	Users = 1,
	Groups = 2,
	Computers = 4,
	Contacts = 8,
	BuiltInGroups = 0x10,
	WellKnownPrincipals = 0x20,
	All = 0x3F
}
