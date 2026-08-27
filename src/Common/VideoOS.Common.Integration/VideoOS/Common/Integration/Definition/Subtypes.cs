using System;

namespace VideoOS.Common.Integration.Definition;

[Flags]
public enum Subtypes
{
	None = 0,
	Standard = 1,
	InterconnectedProduct = 2,
	MipDriver = 4,
	MipArculesDriver = 8
}
