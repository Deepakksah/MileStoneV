using System;

namespace VideoOS.Platform.SDK.Config;

public enum ServerTypes
{
	[Obsolete("Server type is obsolete")]
	ENTERPRISE = 0,
	CORPORATE = 1,
	DATABASE = 2,
	[Obsolete("Server type is obsolete")]
	ARCUS = 3,
	[Obsolete("Server type is obsolete")]
	MOBILE = 4,
	UNKNOWN = 99
}
