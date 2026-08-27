using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectorOAuth
{
	Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoTokenExchange siteConnectionInfoTokenExchange, CancellationToken cancellationToken);

	Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken);
}
