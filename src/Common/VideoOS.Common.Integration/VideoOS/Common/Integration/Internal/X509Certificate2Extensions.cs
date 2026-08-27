using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace VideoOS.Common.Integration.Internal;

public static class X509Certificate2Extensions
{
	private static class WellKnownOidValues
	{
		public const string EnhancedKeyUsage = "2.5.29.37";

		public const string ServerAuthentication = "1.3.6.1.5.5.7.3.1";

		public const string ClientAuthentication = "1.3.6.1.5.5.7.3.2";
	}

	public static IEnumerable<EnhancedKeyUsage> GetEnhancedKeyUsages(this X509Certificate2 certificate)
	{
		if (certificate == null)
		{
			throw new ArgumentNullException("certificate");
		}
		HashSet<EnhancedKeyUsage> hashSet = new HashSet<EnhancedKeyUsage>();
		X509ExtensionEnumerator enumerator = certificate.Extensions.GetEnumerator();
		while (enumerator.MoveNext())
		{
			X509Extension current = enumerator.Current;
			if (!string.Equals(current.Oid.Value, "2.5.29.37", StringComparison.Ordinal))
			{
				continue;
			}
			OidEnumerator enumerator2 = ((X509EnhancedKeyUsageExtension)current).EnhancedKeyUsages.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				string value = enumerator2.Current.Value;
				if (!(value == "1.3.6.1.5.5.7.3.1"))
				{
					if (value == "1.3.6.1.5.5.7.3.2")
					{
						hashSet.Add(EnhancedKeyUsage.ClientAuthentication);
					}
				}
				else
				{
					hashSet.Add(EnhancedKeyUsage.ServerAuthentication);
				}
			}
			break;
		}
		return hashSet;
	}
}
