using System;
using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

[Serializable]
public class ImageServerGroupPresenter : IPresenter
{
	private readonly ImageServerGroupPresenterImplementer _implementer;

	public ImageServerGroupPresenter()
	{
		_implementer = new ImageServerGroupPresenterImplementer();
	}

	public string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args)
	{
		return _implementer.LocalizedFormattableString(key, uiCulture, args);
	}

	public string LocalizedString(string key, CultureInfo uiCulture)
	{
		return _implementer.LocalizedString(key, uiCulture);
	}
}
