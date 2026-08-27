using System;
using System.Net;

namespace VideoOS.Utilities.Windows.Http;

public interface IHttpSysConfig : IHttpSysConfigReader, IDisposable
{
	void EnforceSslConfiguration(ISslConfiguration configuration);

	void DeleteSslConfiguration(ISslConfigurationKey configurationKey);

	void DeleteSslConfiguration(IPAddress address, ushort port);

	void EnforceUrlReservation(UrlPrefix prefix, params UrlReservationAccessControlEntry[] accessControlEntries);

	void DeleteUrlReservation(UrlPrefix prefix);
}
