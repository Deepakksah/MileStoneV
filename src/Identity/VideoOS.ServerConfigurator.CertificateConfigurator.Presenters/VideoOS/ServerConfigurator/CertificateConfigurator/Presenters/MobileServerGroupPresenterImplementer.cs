using System;
using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

public class MobileServerGroupPresenterImplementer : IPresenter
{
	public string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args)
	{
		throw new NotImplementedException();
	}

	public string LocalizedString(string key, CultureInfo uiCulture)
	{
		return MobileServerGroupResource.ResourceManager.GetString(key, uiCulture) ?? ("[" + key + "]");
	}
}
