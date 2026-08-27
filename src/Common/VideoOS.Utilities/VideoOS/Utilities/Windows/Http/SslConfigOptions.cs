using System;

namespace VideoOS.Utilities.Windows.Http;

[Flags]
public enum SslConfigOptions
{
	None = 0,
	NegotiateClientCertificate = 1,
	NoRawFilter = 2,
	UseDSMapper = 4
}
