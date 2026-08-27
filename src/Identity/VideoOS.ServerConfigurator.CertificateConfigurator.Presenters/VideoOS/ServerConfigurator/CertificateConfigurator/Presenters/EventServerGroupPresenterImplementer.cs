using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

public class EventServerGroupPresenterImplementer : IPresenter
{
	public string LocalizedString(string key, CultureInfo uiCulture)
	{
		return EventServerGroupResource.ResourceManager.GetString(key, uiCulture) ?? ("[" + key + "]");
	}

	public string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args)
	{
		return EventServerGroupResource.ResourceManager.GetString(key, uiCulture) ?? ("[" + key + "]");
	}
}
