using System;
using System.Globalization;
using System.Net;

namespace VideoOS.WebService.Common;

public static class SpnFactory
{
	private const string SpnTemplate = "VideoOS/{0}:{1}";

	private static string _localHostFqdn;

	public static string GetSpn(Uri serverUri)
	{
		if (serverUri == null)
		{
			throw new ArgumentNullException("serverUri");
		}
		string text = serverUri.Host;
		if (text.Equals("localhost", StringComparison.OrdinalIgnoreCase))
		{
			if (string.IsNullOrEmpty(_localHostFqdn))
			{
				_localHostFqdn = Dns.GetHostEntry("localhost").HostName;
			}
			text = _localHostFqdn;
		}
		return string.Format(CultureInfo.InvariantCulture, "VideoOS/{0}:{1}", text, serverUri.Port);
	}
}
