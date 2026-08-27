using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Management.VmoClient;

public interface ISiteConnectorLegacy
{
	Task<ISiteConnectionResult> GetSiteConnectionResult(ISiteConnectionInfoWindows siteConnectionInfoWindows, CancellationToken cancellationToken);
}
