using System.Net;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectionInfoWindows : ISiteConnectionInfoBase
{
	ICredentials Credentials { get; }

	bool ForceLegacy { get; }
}
