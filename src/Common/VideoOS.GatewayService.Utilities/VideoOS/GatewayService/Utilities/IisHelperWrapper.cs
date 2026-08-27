using Microsoft.Extensions.Logging;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.GatewayService.Utilities;

public class IisHelperWrapper : IIisHelperWrapper
{
	private ILogger _logComponent;

	private IisHelper _iisHelper;

	public IisHelperWrapper(ILogger logComponent)
	{
		_logComponent = logComponent;
		_iisHelper = new IisHelper(logComponent);
	}

	public bool CheckSecureBindings(string websiteId)
	{
		return _iisHelper.CheckSecureBindings(websiteId);
	}

	public void Control(IisAction action)
	{
		_iisHelper.Control(action);
	}

	public void CreateCertificateAndHttpsBindingIfNeeded(string websiteId)
	{
		_iisHelper.CreateCertificateAndHttpsBindingIfNeeded(websiteId);
	}

	public void GrantAccessToIis(string userName)
	{
		IisHelper.GrantAccessToIis(userName);
	}

	public void CreateCertificateAndHttpsBindingIfNeeded(string websiteId, string certificateHash, bool ignoreExistingCertificate)
	{
		_iisHelper.CreateCertificateAndHttpsBindingIfNeeded(websiteId, certificateHash, ignoreExistingCertificate);
	}

	public IisWebsite[] GetIisWebsites(ILogger logger)
	{
		return IisHelper.GetIisWebsites(logger);
	}
}
