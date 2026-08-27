using Microsoft.Extensions.Logging;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public interface IIisHelperWrapper
{
	bool CheckSecureBindings(string websiteId);

	void CreateCertificateAndHttpsBindingIfNeeded(string websiteId);

	void Control(IisAction action);

	void GrantAccessToIis(string userName);

	IisWebsite[] GetIisWebsites(ILogger logger);

	void CreateCertificateAndHttpsBindingIfNeeded(string websiteId, string certificateHash, bool ignoreExistingCertificate);
}
