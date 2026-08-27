using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public interface IFreeLicenseDownloader
{
	Task<LicenseJsonResponse> DownloadAsync(CancellationToken token);
}
