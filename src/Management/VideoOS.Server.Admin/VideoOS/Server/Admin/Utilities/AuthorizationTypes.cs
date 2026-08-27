using System;

namespace VideoOS.Server.Admin.Utilities;

[Flags]
public enum AuthorizationTypes
{
	None = 0,
	Basic = 1,
	Ntlm = 2,
	Digest = 4,
	Anonymous = 8
}
