using System;

namespace VideoOS.Server.Admin.Utilities;

[Flags]
public enum AccessRightTypes
{
	None = 0,
	Read = 1,
	Execute = 2,
	Write = 4,
	Script = 8
}
