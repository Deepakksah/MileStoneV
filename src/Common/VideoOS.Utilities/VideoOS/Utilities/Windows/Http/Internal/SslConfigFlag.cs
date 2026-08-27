using System;

namespace VideoOS.Utilities.Windows.Http.Internal;

[Flags]
public enum SslConfigFlag
{
	None = 0,
	UseDSMapper = 1,
	NegotiateClientCertificates = 2,
	DoNotRouteToRawIsapiFilters = 4
}
