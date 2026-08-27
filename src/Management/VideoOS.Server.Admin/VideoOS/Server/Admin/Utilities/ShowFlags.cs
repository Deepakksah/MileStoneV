using System;

namespace VideoOS.Server.Admin.Utilities;

[Flags]
public enum ShowFlags
{
	None = 0,
	Date = 2,
	Time = 4,
	Size = 8,
	Extension = 0x10,
	LongDate = 0x20
}
