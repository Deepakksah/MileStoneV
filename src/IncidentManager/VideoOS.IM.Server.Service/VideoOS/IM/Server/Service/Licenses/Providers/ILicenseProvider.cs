using System.Threading.Tasks;
using VideoOS.IM.Server.Service.Licenses.DTOs;

namespace VideoOS.IM.Server.Service.Licenses.Providers;

public interface ILicenseProvider
{
	Task<LicenseInformationResponse> GetLicenseAsync();
}
