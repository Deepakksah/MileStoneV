using System;
using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

[Serializable]
public class ServerToServerGroupPresenter : IPresenter
{
	public string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args)
	{
		throw new NotImplementedException();
	}

	public string LocalizedString(string key, CultureInfo uiCulture)
	{
		return ServerToServerGroupResource.ResourceManager.GetString(key, uiCulture) ?? ("[" + key + "]");
	}
}
