using System;
using System.Globalization;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public class FakePresenter : IPresenter
{
	public string LocalizedFormattableString(string key, CultureInfo uiCulture, params object[] args)
	{
		try
		{
			return string.Format(CultureInfo.CurrentCulture, key, args);
		}
		catch (FormatException)
		{
			return key + " args: " + string.Join(", ", args);
		}
	}

	public string LocalizedString(string key, CultureInfo uiCulture)
	{
		return key;
	}
}
