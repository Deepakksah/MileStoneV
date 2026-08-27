using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnector
{
	int DiscoveryTimeout { get; }

	ISiteConnectionResult GetSiteConnection(ISiteConnectionInfoWindows siteConnectionInfoWindows);

	ISiteConnectionResult GetSiteConnection(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange);

	Task<ISiteConnectionResult> GetSiteConnectionAsync(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken);

	Task<ISiteConnectionResult> GetSiteConnectionAsync(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange, CancellationToken cancellationToken);

	void CheckForProtocolAllowed(string scheme, bool allowNonSecure);

	string GetUserDomainNameOrEmpty(NetworkCredential networkCredential);
}
