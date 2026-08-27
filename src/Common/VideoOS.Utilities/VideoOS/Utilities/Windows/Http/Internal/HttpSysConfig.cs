using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading;

namespace VideoOS.Utilities.Windows.Http.Internal;

public class HttpSysConfig : IHttpSysConfig, IHttpSysConfigReader, IDisposable
{
	private IDisposable _initializationToken;

	private IWin32Http Win32Http { get; }

	public HttpSysConfig(IWin32Http nativeHelper)
	{
		Win32Http = nativeHelper;
		_initializationToken = Win32Http.InitializeConfig();
	}

	public HttpSysConfig()
		: this(Win32HttpNativeMethods.Instance)
	{
	}

	public void Dispose()
	{
		Interlocked.Exchange(ref _initializationToken, null)?.Dispose();
	}

	IEnumerable<ISslConfiguration> IHttpSysConfigReader.ReadSslConfigurations()
	{
		return from c in Win32Http.QuerySslConfigurations()
			select new SslConfigurationWrapper(c);
	}

	public IEnumerable<IUrlReservationInfoCollection> ReadUrlReservationInfo()
	{
		return from c in Win32Http.QueryUrlAclInfos()
			select new UrlReservationInfoCollectionWrapper(c);
	}

	public void EnforceSslConfiguration(ISslConfiguration configuration)
	{
		SslConfiguration sslConfiguration = ConvertConfiguration(configuration);
		SslConfiguration sslConfiguration2 = Win32Http.QuerySslConfiguration(sslConfiguration);
		if (sslConfiguration2 != null)
		{
			Win32Http.DeleteSslConfiguration(sslConfiguration2);
		}
		Win32Http.SetSslConfiguration(sslConfiguration);
	}

	public void DeleteSslConfiguration(ISslConfigurationKey configurationKey)
	{
		if (configurationKey == null)
		{
			throw new ArgumentNullException("configurationKey");
		}
		DeleteSslConfiguration(configurationKey.Address, configurationKey.Port);
	}

	public void DeleteSslConfiguration(IPAddress address, ushort port)
	{
		if (address == null)
		{
			throw new ArgumentNullException("address");
		}
		if (port == 0)
		{
			throw new ArgumentOutOfRangeException("port");
		}
		SslConfigurationKey configurationKey = new SslConfigurationKey(address, port);
		Win32Http.DeleteSslConfiguration(configurationKey);
	}

	public void EnforceUrlReservation(UrlPrefix prefix, params UrlReservationAccessControlEntry[] accessControlEntries)
	{
		if (prefix == null)
		{
			throw new ArgumentNullException("prefix");
		}
		if (accessControlEntries == null)
		{
			throw new ArgumentNullException("accessControlEntries");
		}
		if (accessControlEntries.Length == 0)
		{
			throw new ArgumentException("At least one ACL entry must be provided", "accessControlEntries");
		}
		string urlPrefix = prefix.ToString();
		if (Win32Http.QueryUrlAclInfo(urlPrefix) != null)
		{
			Win32Http.DeleteUrlAclInfo(urlPrefix);
		}
		try
		{
			Win32Http.SetUrlAclInfo(UrlReservationInfoCollectionWrapper.ToUrlAclInfo(prefix, accessControlEntries));
			return;
		}
		catch (Win32Exception ex)
		{
			if (ex.NativeErrorCode != 183)
			{
				throw;
			}
		}
		UrlPrefix[] array = (from u in Win32Http.QueryUrlAclInfos()
			select new UrlReservationInfoCollectionWrapper(u).UrlReservation into r
			where r.Port == prefix.Port && r.Scheme != prefix.Scheme
			select r).ToArray();
		foreach (UrlPrefix urlPrefix2 in array)
		{
			Win32Http.DeleteUrlAclInfo(urlPrefix2.ToString());
		}
		Win32Http.SetUrlAclInfo(UrlReservationInfoCollectionWrapper.ToUrlAclInfo(prefix, accessControlEntries));
	}

	public void DeleteUrlReservation(UrlPrefix prefix)
	{
		if (prefix == null)
		{
			throw new ArgumentNullException("prefix");
		}
		Win32Http.DeleteUrlAclInfo(prefix.ToString());
	}

	private static SslConfiguration ConvertConfiguration(ISslConfiguration configuration)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		return new SslConfiguration(configuration.Port, configuration.Address, configuration.ApplicationId, configuration.CertificateStoreName, configuration.GetCertificateHash(), configuration.DefaultRevocationFreshnessTime, configuration.DefaultRevocationUrlRetrievalTimeout, configuration.DefaultSslCtlIdentifier, configuration.DefaultSslCtlStoreName, configuration.DefaultCertCheckMode.ToNative(), configuration.DefaultFlags.ToNative());
	}
}
