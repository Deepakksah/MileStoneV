using System;
using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI.Globalization;

[CLSCompliant(true)]
public interface ICultureProvider
{
	IEnumerable<CultureInfo> SupportedCultures { get; }

	IEnumerable<Exception> Exceptions { get; }
}
