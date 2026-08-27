using System;

namespace VideoOS.Server.Admin.Utilities;

[Flags]
public enum SslFlags
{
	None = 0,
	Ssl = 8,
	SslNegotiateCert = 0x20,
	SslRequireCert = 0x40,
	SslMapCert = 0x80,
	Ssl128 = 0x100
}
