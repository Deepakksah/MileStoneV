using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

public class MobileServerGroupPresenter : IPresenter
{
	private readonly MobileServerGroupPresenterImplementer _implementer;

	public MobileServerGroupPresenter()
	{
		_implementer = new MobileServerGroupPresenterImplementer();
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
