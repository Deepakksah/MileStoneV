using System.Net;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionInfo
{
	ISite Site { get; }

	ICredentials Credentials { get; }

	string TrustedJwtAccessToken { get; }

	bool AllowUnsecure { get; }

	bool ForceLegacy { get; }
}
