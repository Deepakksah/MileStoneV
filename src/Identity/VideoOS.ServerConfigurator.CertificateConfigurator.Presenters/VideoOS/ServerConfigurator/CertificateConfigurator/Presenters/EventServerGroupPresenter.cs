using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.Presenters;

public class EventServerGroupPresenter : IPresenter
{
	private readonly IPresenter _implementer;

	public EventServerGroupPresenter()
	{
		_implementer = new EventServerGroupPresenterImplementer();
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
