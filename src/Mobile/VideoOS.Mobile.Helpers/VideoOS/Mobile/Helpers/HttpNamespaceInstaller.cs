using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceProcess;
using VideoOS.Utilities.Windows.Http;

namespace VideoOS.Mobile.Helpers;

public static class HttpNamespaceInstaller
{
	private static Dictionary<ServiceAccount, WellKnownSidType> _serviceAddToWellKnowSid = new Dictionary<ServiceAccount, WellKnownSidType>
	{
		{
			ServiceAccount.LocalService,
			WellKnownSidType.LocalServiceSid
		},
		{
			ServiceAccount.LocalSystem,
			WellKnownSidType.LocalSystemSid
		},
		{
			ServiceAccount.NetworkService,
			WellKnownSidType.NetworkServiceSid
		}
	};

	public static string GetBuiltInAccountName(string mosServiceUsername)
	{
		mosServiceUsername = mosServiceUsername ?? throw new ArgumentNullException("mosServiceUsername");
		string value = ((NTAccount)new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null).Translate(typeof(NTAccount))).Value;
		string value2 = ((NTAccount)new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null).Translate(typeof(NTAccount))).Value;
		string value3 = ((NTAccount)new SecurityIdentifier(WellKnownSidType.LocalServiceSid, null).Translate(typeof(NTAccount))).Value;
		if (mosServiceUsername.Equals(value, StringComparison.OrdinalIgnoreCase))
		{
			return ServiceAccount.NetworkService.ToString();
		}
		if (mosServiceUsername.Equals(value2, StringComparison.OrdinalIgnoreCase))
		{
			return ServiceAccount.LocalSystem.ToString();
		}
		if (mosServiceUsername.Equals(value3, StringComparison.OrdinalIgnoreCase))
		{
			return ServiceAccount.LocalService.ToString();
		}
		return "";
	}

	public static string GetServiceUserName(string accountName, bool isUserAccount)
	{
		ServiceAccount serviceAccount = (isUserAccount ? ServiceAccount.User : ((ServiceAccount)Enum.Parse(typeof(ServiceAccount), accountName)));
		if (serviceAccount != ServiceAccount.User)
		{
			return ((NTAccount)new SecurityIdentifier(_serviceAddToWellKnowSid[serviceAccount], null).Translate(typeof(NTAccount))).Value;
		}
		return accountName;
	}

	public static void SetReservation(string scheme, ushort port, string accountName)
	{
		accountName = accountName ?? throw new ArgumentNullException("accountName");
		using IHttpSysConfig config = HttpConfigFactory.Instance.CreateConfig();
		SetReservation(config, scheme, port, accountName);
	}

	public static void SetReservation(IHttpSysConfig config, string scheme, ushort port, string accountName)
	{
		if (accountName.StartsWith(".\\", StringComparison.Ordinal))
		{
			accountName = Environment.MachineName + accountName.TrimStart('.');
		}
		UrlScheme scheme2 = ((!(scheme == Uri.UriSchemeHttp)) ? UrlScheme.Https : UrlScheme.Http);
		SecurityIdentifier identity = new NTAccount(accountName).Translate(typeof(SecurityIdentifier)) as SecurityIdentifier;
		UrlPrefix prefix = UrlPrefix.CreateStrongWildcard(scheme2, port, "/");
		UrlReservationAccessControlEntry urlReservationAccessControlEntry = new UrlReservationAccessControlEntry(identity, allowListen: true, allowDelegate: false);
		config?.EnforceUrlReservation(prefix, urlReservationAccessControlEntry);
	}

	public static void DeleteReservation(string scheme, ushort port)
	{
		using IHttpSysConfig config = HttpConfigFactory.Instance.CreateConfig();
		DeleteReservation(config, scheme, port);
	}

	public static void DeleteReservation(IHttpSysConfig config, string scheme, ushort port)
	{
		UrlPrefix prefix = UrlPrefix.CreateStrongWildcard((!(scheme == Uri.UriSchemeHttp)) ? UrlScheme.Https : UrlScheme.Http, port, "/");
		config?.DeleteUrlReservation(prefix);
	}

	public static void DeleteReservationNoThrow(string scheme, ushort port)
	{
		try
		{
			DeleteReservation(scheme, port);
		}
		catch (Exception)
		{
		}
	}
}
