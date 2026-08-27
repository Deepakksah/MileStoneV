using System.Globalization;

namespace VideoOS.ServerConfigurator.CertificateRegistration;

public interface IPresenter
{
	string LocalizedString(string key, CultureInfo uiCulture);

	string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args);
}
