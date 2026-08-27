using System.Globalization;

namespace VideoOS.Server.Failover.Configurator;

public static class WebConsoleHelper
{
	public static string GetWebConsoleAddress(string fqdn, bool useHttps)
	{
		if (!useHttps)
		{
			return string.Format(CultureInfo.InvariantCulture, "http://{0}:9010", fqdn);
		}
		return string.Format(CultureInfo.InvariantCulture, "https://{0}:9453", fqdn);
	}
}
